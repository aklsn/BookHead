using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTestD : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    public float bounceAmplitude = 0.1f;
    public float bounceFrequency = 5f;

    private Rigidbody _rb;
    private Vector3 _inputDirection;
    private Vector3 _initialCameraPosition;
    private float _bounceTimer;
    private float _xRotation = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;


        Cursor.lockState = CursorLockMode.Locked;
        mouseSensitivity = GameManager_J.Instance.mouseSensitivity;
        if (cameraTransform != null)
        {
            _initialCameraPosition = cameraTransform.localPosition;
        }
    }

    private void Update()
    {
        HandleMouseLook();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 mov = new Vector3(h, 0, v);
        this.transform.Translate(mov * Time.deltaTime * playerSpeed);
        _inputDirection = new Vector3(h, 0.0f, v).normalized;



        if (Input.GetMouseButtonDown(0)) // 마우스 클릭
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    doorControl door = hit.collider.GetComponent<doorControl>();

                    if (door != null)
                    {
                        door.ChangeDoorState(); // 문 상태 변경 호출
                    }
                }
            }
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

    // 움직임이 있을 때만 흔들림 효과를 적용
    if (_inputDirection.magnitude > 0.1f)
    {
        _bounceTimer += Time.deltaTime * bounceFrequency;
        float bounceOffset = Mathf.Sin(_bounceTimer) * bounceAmplitude;

        // Y축 흔들림 적용
        Vector3 currentPosition = cameraTransform.localPosition;
        currentPosition.y = Mathf.Lerp(currentPosition.y, _initialCameraPosition.y + bounceOffset, Time.deltaTime * 10f);
        cameraTransform.localPosition = currentPosition;
    }
    else
    {
        // 움직임이 없으면 원래 위치로 복귀
        Vector3 currentPosition = cameraTransform.localPosition;
        currentPosition.y = Mathf.Lerp(currentPosition.y, _initialCameraPosition.y, Time.deltaTime * 10f);
        cameraTransform.localPosition = currentPosition;
    }
}


    private void HandleMouseLook()
    {
       float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); 


        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);


        transform.Rotate(Vector3.up * mouseX);
    }
}
