using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerControl : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    public Transform Spotlight;
    public float bounceAmplitude = 0.1f;
    public float bounceFrequency = 5f;

    private Rigidbody _rb;
    private Vector3 _inputDirection;
    private Vector3 _initialCameraPosition;
    private float _bounceTimer;
    private float _xRotation = 0f;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        _rb = GetComponent<Rigidbody>();

        mouseSensitivity = GameManager_J.Instance.mouseSensitivity;

        Cursor.lockState = CursorLockMode.Locked;

        if (cameraTransform != null)
        {
            _initialCameraPosition = cameraTransform.localPosition;
        }
    }

    private void Update()
    {
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

        // 플레이어 이동 입력 처리
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        _inputDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // 마우스 회전 처리
        HandleMouseLook();

        // 마우스 클릭 처리
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    private void FixedUpdate()
    {
        // 플레이어 이동 처리
        Vector3 moveDirection = transform.TransformDirection(_inputDirection);

        Vector3 move = moveDirection * playerSpeed * Time.deltaTime;
        _rb.MovePosition(transform.position + move);

        // 카메라 바운스 처리
        HandleCameraBounce();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        Spotlight.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float maxDistance = 1f; // 레이캐스트 최대 거리 (1미터)

        if (Physics.Raycast(ray, out hit, maxDistance)) // 최대 거리 추가
        {
            if (hit.collider != null && hit.collider.CompareTag("Door"))
            {
                Door door = hit.collider.GetComponent<Door>();
                if (door != null)
                {
                    door.ChangeDoorState();
                    Debug.Log("Door clicked and state changed!");
                }
            }
        }
        else
        {
            Debug.Log("No interactable object within 1 meter.");
        }
    }

    private void HandleCameraBounce()
    {
        if (cameraTransform == null) return;
        if (_inputDirection.magnitude > 0.1f)
        {
            _bounceTimer += Time.deltaTime * bounceFrequency;
            float bounceOffset = Mathf.Sin(_bounceTimer) * bounceAmplitude;
            cameraTransform.localPosition = _initialCameraPosition + new Vector3(0, bounceOffset, 0);
        }
        else
        {
            cameraTransform.localPosition = _initialCameraPosition;
        }
    }
}
