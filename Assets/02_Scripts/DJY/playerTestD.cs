using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class playerTestD : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    public float bounceAmplitude = 0.1f;
    public float bounceFrequency = 5f;
    public float cameraOffset = 1f;

    private Rigidbody _rb;
    private Vector3 _inputDirection;
    private Vector3 _initialCameraPosition;
    private float _bounceTimer;
    private float _xRotation = 0f;

    public Image crosshair;       // 조준점 UI 이미지
    public Sprite defaultSprite; // 기본 조준점 이미지
    public Sprite interactSprite; // 상호작용 조준점 이미지

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
        mouseSensitivity = GameManager_J.Instance.mouseSensitivity;
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

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 mov = new Vector3(h, 0, v);
        this.transform.Translate(mov * Time.deltaTime * playerSpeed);
        _inputDirection = new Vector3(h, 0.0f, v).normalized;

        HandleMouseLook();

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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);


        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);


        transform.Rotate(Vector3.up * mouseX);
    }
}
