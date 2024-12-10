using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;

public class playerTestD : MonoBehaviour
{
    //플레이어 기능
    public float playerSpeed = 5f;
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    public float bounceAmplitude = 0.1f;
    public float bounceFrequency = 5f;
    public float cameraOffset = 1f;

    //조준점
    public Image crosshair;
    public Sprite defaultSprite;
    public Sprite interactSprite;

    private Rigidbody _rb;
    private Vector3 _inputDirection;
    private Vector3 _initialCameraPosition;
    private float _bounceTimer;
    private float _xRotation = 0f;

    //캐릭터 화면 전환(댐핑)
    private float currentMouseX;
    private float currentMouseY;
    private float mouseXVelocity;
    private float mouseYVelocity;
    private float smoothTime = 0.1f;

    //발소리
    public AudioSource footAudioSource;
    public AudioClip footAudioClip;
    private bool isMoving = false;
    private bool isfootAudioPlay = false;


    public AudioSource S_Door;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        //mouseSensitivity = GameManager_J.Instance.mouseSensitivity;
        if (cameraTransform != null)
        {
            _initialCameraPosition = cameraTransform.localPosition;
        }
    }

    private void Update()
    {
        //mouseSensitivity = GameManager_J.Instance.mouseSensitivity;
        // 옵션 창이 열려 있는 경우
        if (OptionController_J.Instance != null && OptionController_J.Instance.IsOptionOpen())
        {
            // UI 위에서 클릭이 발생했는지 확인
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                // UI 클릭 이벤트는 허용
                return;
            }

            // UI 밖에서 클릭 시 게임 입력 차단
            return;
        }

        FootSteps();
        HandleMouseLook();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 mov = new Vector3(h, 0, v);
        this.transform.Translate(mov * Time.deltaTime * playerSpeed);
        _inputDirection = new Vector3(h, 0.0f, v).normalized;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float maxDistance = 1.5f; // 레이캐스트 최대 거리 (1미터)

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Door") || hit.collider.CompareTag("Bed") || hit.collider.CompareTag("Light") ||
                hit.collider.CompareTag("Lp") || hit.collider.CompareTag("Tvcontroller"))
            {
                crosshair.sprite = interactSprite;
            }
            else
            {
                crosshair.sprite = defaultSprite;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("Door")) // 마우스 클릭
                {
                    // S_Door.Play();
                    doorController door = hit.collider.GetComponent<doorController>();

                    if (door != null)
                    {
                        door.ChangeDoorState(); // 문 상태 변경 호출
                    }
                }
                else if (hit.collider.CompareTag("Bed"))
                {
                    BedScript_Raccoon bed = hit.collider.GetComponent<BedScript_Raccoon>();
                    if (bed != null && bed.IsEventOn == true)
                    {
                        bed.IsClick = true;
                    }
                }
                else if (hit.collider.CompareTag("Light"))
                {
                    active_light light = hit.collider.GetComponent<active_light>();
                    if (light != null)
                    {
                        light.ChangeLightState();
                    }
                }
                else if (hit.collider.CompareTag("Lp"))
                {
                    GameObject LP = GameObject.Find("LpTrigger");
                    GameObject DoorControl = GameObject.Find("Door_2B (3)");
                    Transform ChilDoor = DoorControl.transform.Find("Interior_Door");
                    doorController Door = ChilDoor.GetComponent<doorController>();
                    LPTrigger lp = hit.collider.GetComponent<LPTrigger>();
                    
                    if (lp != null)
                    {
                        if (lp.IsLp)
                        {
                            Door.CloseControl = false;
                            Door.open = true;
                            LP.SetActive(false);
                        }
                        lp.IsDone = true;
                        lp.IsLp = false;
                    }
                }
                else if (hit.collider.CompareTag("Tvcontroller"))
                {
                    GameObject TV = GameObject.Find("TvTrigger");
                    GameObject TvNoise = GameObject.Find("TvNoise");
                    TVTrigger tv = hit.collider.GetComponent<TVTrigger>();
                    if (tv != null)
                    {
                        if (tv.IsTv)
                        {
                            TvNoise.SetActive(false);
                            TV.SetActive(false);
                        }

                        tv.IsDone = true;
                        tv.IsTv = false;
                    }
                }
            }
        }
        else
        {
            crosshair.sprite = defaultSprite;
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.TransformDirection(_inputDirection);

        Vector3 move = moveDirection * playerSpeed * Time.deltaTime;
        _rb.MovePosition(transform.position + move);

        HandleCameraBounce();
    }

    private void HandleCameraBounce()
    {
        if (cameraTransform == null) return;
        if (_inputDirection.magnitude > 0.1f)
        {
            _bounceTimer += Time.deltaTime * bounceFrequency;
            float bounceOffset = Mathf.Sin(_bounceTimer) * bounceAmplitude;
            cameraTransform.localPosition = _initialCameraPosition + new Vector3(0, bounceOffset + cameraOffset, 0);
        }
        else
        {
            cameraTransform.localPosition = _initialCameraPosition + new Vector3(0, cameraOffset, 0);
        }
    }

    private void HandleMouseLook()
    {
        float targetMouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 0.01f;
        float targetMouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 0.01f;

        // SmoothDamp
        currentMouseX = Mathf.Lerp(currentMouseX, targetMouseX, smoothTime);
        currentMouseY = Mathf.Lerp(currentMouseY, targetMouseY, smoothTime);

        _xRotation -= currentMouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * currentMouseX);
    }

    private void FootSteps()
    {
        isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (isMoving)
        {
            if (!isfootAudioPlay)
            {
                footAudioSource.clip = footAudioClip;
                footAudioSource.loop = true;
                footAudioSource.Play();
                isfootAudioPlay = true;
            }
        }
        else
        {
            if (isfootAudioPlay)
            {
                StartCoroutine(StopAudioWithDelay(.3f)); // 1초 뒤에 소리 종료
            }
        }
    }

    private IEnumerator StopAudioWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        footAudioSource.Stop();
        isfootAudioPlay = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("lpTrigger"))
        {
            GameObject lpPlayer = GameObject.FindWithTag("Lp");
            LPTrigger lp = lpPlayer.GetComponent<LPTrigger>();

            // 사운드가 재생 중인지 확인
            if (!lp.sound1.isPlaying)
            {
                lp.IsLp = true;
                lp.sound1.Play(); // 사운드 재생
            }
        }

        if (other.gameObject.name == "TvTrigger")
        {
            GameObject TV = GameObject.Find("SM_TV_29");
            GameObject remote = GameObject.Find("SM_Remote_11");
            if (TV != null)
            {
                TVTrigger tv = remote.GetComponent<TVTrigger>();
                if (tv != null)
                {
                    if (!tv.sound1.isPlaying)
                    {
                        tv.IsTv = true;
                        tv.sound1.Play();
                    }
                    Transform tvNoiseTransform = TV.transform.Find("TvNoise");
                    if (tvNoiseTransform != null)
                    {
                        GameObject tvNoiseObject = tvNoiseTransform.gameObject;
                        tvNoiseObject.SetActive(true);
                    }
                }
            }
        }
        if (other.gameObject.name == "SoundTriggerPoint1")
        {
            GameObject doorlook = GameObject.Find("doorLockManager");
            LockableDoorManager1 dr = doorlook.GetComponent<LockableDoorManager1>();

            dr.LockDoor();
            GameObject doorTrigger = GameObject.Find("SoundTriggerPoint1");
            doorTrigger.SetActive(false);
        }

        if (other.gameObject.name == "SoundTriggerPoint2")
        {
            GameObject doorlook2 = GameObject.Find("doorLockManager2");
            LockableDoorManager1 dr = doorlook2.GetComponent<LockableDoorManager1>();

            dr.LockDoor();
            GameObject doorTrigger = GameObject.Find("SoundTriggerPoint2");
            doorTrigger.SetActive(false);
        }
    }
}

