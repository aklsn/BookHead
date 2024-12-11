using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Insect_VFX;
using UnityEngine.Rendering.Universal;

public class playerTestJ : MonoBehaviour
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

    public Camera playerCamera;         // 플레이어 카메라
    public Camera headCamera;           // 머리 앞 카메라
    private Camera currentCamera;
    public float insectDisplayTime = 1.5f; // 머리 앞 카메라 유지 시간

    public AudioSource S_Door;

    private bool is_on = false;

    public GameObject[] emitterObjects;
    public GameObject linkedDoor;
    private void Start()
    {
        currentCamera = playerCamera;
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

        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float maxDistance = 1.5f; // 레이캐스트 최대 거리 (1미터)

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Door") || hit.collider.CompareTag("Bed") || hit.collider.CompareTag("Light") || hit.collider.CompareTag("Insect"))
            {
                crosshair.sprite = interactSprite;
            }
            else
            {
                crosshair.sprite = defaultSprite;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("Door")) // 문 클릭 처리
                {
                    S_Door.Play();
                    doorController door = hit.collider.GetComponent<doorController>();

                    if (door != null)
                    {
                        door.ChangeDoorState(); // 문 상태 변경 호출
                    }
                }
                else if (hit.collider.CompareTag("Bed")) // 침대 클릭 처리
                {
                    BedScript_Raccoon bed = hit.collider.GetComponent<BedScript_Raccoon>();
                    if (bed != null && bed.IsEventOn == true)
                    {
                        bed.IsClick = true;
                    }
                }
                else if (hit.collider.CompareTag("Light")) // 조명 클릭 처리
                {
                    active_light light = hit.collider.GetComponent<active_light>();
                    if (light != null)
                    {
                        light.ChangeLightState();
                    }
                }
                else if (hit.collider.CompareTag("Insect") && is_on == false) // 벌레 클릭 처리
                {
                    is_on = true;

                    InsectEmitter emitter = hit.collider.GetComponent<InsectEmitter>();
                    if (emitter != null)
                    {
                        StartCoroutine(SwitchToHeadCameraAndBack(emitter));
                    }

                    StartEmitters();
                    UnlockDoor();
                }
            }
        }
        else
        {
            crosshair.sprite = defaultSprite;
        }
    }
    private void UnlockDoor()
    {
        doorController door = linkedDoor.GetComponent<doorController>();
        if (door != null)
        {
            if (door.CloseControl)
            {
                door.CloseControl = false; // 문 잠금 해제
            }
            else
            {
                Debug.Log($"문 {linkedDoor.name}는 이미 열려 있거나 잠겨 있지 않습니다.");
            }
        }
        else
        {
            Debug.LogError("linkedDoor에 doorController가 없습니다.");
        }
    }
    private void StartEmitters()
    {
        foreach (GameObject emitterObject in emitterObjects)
        {
            InsectEmitter emitter = emitterObject.GetComponent<InsectEmitter>();
            if (emitter != null)
            {
                emitter.StartSimulation();
            }
        }
    }
    private IEnumerator SwitchToHeadCameraAndBack(InsectEmitter emitter)
    {
        Debug.Log("머리 카메라 활성화 시도");

        // Base 카메라 비활성화
        playerCamera.enabled = false;

        // Overlay 카메라 활성화
        headCamera.enabled = true;

        // 카메라 스택 추가
        var playerCameraData = playerCamera.GetUniversalAdditionalCameraData();
        playerCameraData.cameraStack.Add(headCamera);

        Debug.Log("머리 카메라 활성화 완료");

        // 벌레 생성
        //emitter.StartSimulation(insectDisplayTime);
        emitter.StartSimulation();
        yield return new WaitForSeconds(insectDisplayTime);

        Debug.Log("플레이어 카메라로 복귀 시도");

        // Overlay 카메라 비활성화
        headCamera.enabled = false;

        // Base 카메라 활성화
        playerCamera.enabled = true;

        // 카메라 스택에서 제거
        playerCameraData.cameraStack.Remove(headCamera);

        Debug.Log("플레이어 카메라 복귀 완료");
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
}
