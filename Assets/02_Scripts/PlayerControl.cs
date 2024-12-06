using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerControl : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float mouseSensitivity = 0f;
    public Transform cameraTransform;
    //public Transform Spotlight;
    public float bounceAmplitude = 0.1f;
    public float bounceFrequency = 5f;
    public float cameraOffset = 1f;

    private Rigidbody _rb;
    private Vector3 _inputDirection;
    private Vector3 _initialCameraPosition;
    private float _bounceTimer;
    private float _xRotation = 0f;
    private float _yRotation = 0f;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        _rb = GetComponent<Rigidbody>();

        //mouseSensitivity = GameManager_J.Instance.mouseSensitivity;

        Cursor.lockState = CursorLockMode.Locked;

        if (cameraTransform != null)
        {
            _initialCameraPosition = cameraTransform.localPosition;
        }
    }

    private void Update()
    {
        // �ɼ� â�� ���� �ִ� ���
        if (OptionController_J.Instance != null && OptionController_J.Instance.IsOptionOpen())
        {
            // UI ������ Ŭ���� �߻��ߴ��� Ȯ��
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                // UI Ŭ�� �̺�Ʈ�� ���
                return;
            }

            // UI �ۿ��� Ŭ�� �� ���� �Է� ����
            return;
        }

        // �÷��̾� �̵� �Է� ó��
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        _inputDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // ���콺 ȸ�� ó��
        HandleMouseLook();

        // ���콺 Ŭ�� ó��
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    private void FixedUpdate()
    {
        // ī�޶� ���� �̵� ���� ���
        Vector3 cameraForward = cameraTransform.forward; // ī�޶��� ���� ����
        Vector3 cameraRight = cameraTransform.right;     // ī�޶��� ������ ����

        // ���� �̵� ���� (��� �̵��� ���)
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // �Է� ������ ī�޶� ���⿡ ����ȭ
        Vector3 moveDirection = (cameraForward * _inputDirection.z + cameraRight * _inputDirection.x).normalized;

        // �̵� ó��
        Vector3 move = moveDirection * playerSpeed * Time.deltaTime;
        _rb.MovePosition(transform.position + move);

        // ī�޶� �ٿ ó��
        HandleCameraBounce();
    }

    private void HandleMouseLook()
    {
        // ���콺 �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ���� ȸ�� (Y��)
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); // -90�� ~ 90�� ����

        // �¿� ȸ�� (X��)
        _yRotation += mouseX;

        // ī�޶� ȸ�� ����
        cameraTransform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }

    private void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float maxDistance = 1f; // ����ĳ��Ʈ �ִ� �Ÿ� (1����)

        if (Physics.Raycast(ray, out hit, maxDistance)) // �ִ� �Ÿ� �߰�
        {
            if (hit.collider != null)
            {
                if(hit.collider.CompareTag("Door"))
                {
                    Door door = hit.collider.GetComponent<Door>();
                    if (door != null)
                    {
                        door.ChangeDoorState();
                        Debug.Log("Door clicked and state changed!");
                    }
                }
                else if(hit.collider.CompareTag("Bed"))
                {
                    BedScript_Raccoon bed = hit.collider.GetComponent<BedScript_Raccoon>();
                    if ( bed != null && bed.IsEventOn == true )
                    {
                        bed.IsClick = true;
                    }
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
            cameraTransform.localPosition = _initialCameraPosition + new Vector3(0, bounceOffset + cameraOffset, 0) ;
        }
        else
        {
            cameraTransform.localPosition = _initialCameraPosition + new Vector3(0, cameraOffset, 0);
        }
    }
}
