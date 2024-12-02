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
        Cursor.lockState = CursorLockMode.Locked;

        if (cameraTransform != null)
        {
            _initialCameraPosition = cameraTransform.localPosition;
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleCameraBounce();

        if (Input.GetMouseButtonDown(0)) // 마우스 클릭
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    Door door = hit.collider.GetComponent<Door>();

                    if (door != null)
                    {
                        door.ChangeDoorState(); // 문 상태 변경 호출
                    }
                }
            }
        }
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        _inputDirection = new Vector3(h, 0, v).normalized;

        Vector3 move = transform.TransformDirection(_inputDirection) * playerSpeed * Time.deltaTime;
        _rb.MovePosition(transform.position + move);
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

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(-mouseY, 0f, 0f);
        }
    }
}
