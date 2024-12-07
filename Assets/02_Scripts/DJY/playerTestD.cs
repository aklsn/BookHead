using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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



        mouseSensitivity = GameManager_J.Instance.mouseSensitivity * 0.5f;
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
        float maxDistance = 1f; // 레이캐스트 최대 거리 (1미터)

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Door") || hit.collider.CompareTag("Bed"))
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
                    doorControl door = hit.collider.GetComponent<doorControl>();

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
        float targetMouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 0.02f;
        float targetMouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 0.02f;

        // SmoothDamp
        currentMouseX = Mathf.SmoothDamp(currentMouseX, targetMouseX, ref mouseXVelocity, smoothTime);
        currentMouseY = Mathf.SmoothDamp(currentMouseY, targetMouseY, ref mouseYVelocity, smoothTime);

        _xRotation -= currentMouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * currentMouseX);
    }

    private void FootSteps()
    {
        isMoving = Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") !=0;

        if(isMoving)
        {
            if(!isfootAudioPlay)
            {
                footAudioSource.clip = footAudioClip;
                footAudioSource.loop = true;
                footAudioSource.Play();
                isfootAudioPlay = true;
            }
        }else
        {
            if(isfootAudioPlay){
                footAudioSource.Stop();
                isfootAudioPlay = false;
            }
        }
    }

}
