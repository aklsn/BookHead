using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameObject triggerObject;   // Trigger Collider ������Ʈ
    public GameObject targetObject;    // �̵��� ���� ������Ʈ
    public float moveSpeed = 10f;      // �̵� �ӵ�
    public float moveDistance = 50f;  // �̵� �Ÿ�

    private bool eventTriggered = false; // �̺�Ʈ �ߺ� ���� ����
    private Vector3 startPosition;       // �̵� ���� ��ġ
    private Vector3 targetPosition;      // �̵� ��ǥ ��ġ
    private bool isMoving = false;       // �̵� ������ ����

    private void Start()
    {
        if (triggerObject == null)
        {
            Debug.LogError("Trigger Object�� �������� �ʾҽ��ϴ�.");
        }

        if (targetObject == null)
        {
            Debug.LogError("Target Object�� �������� �ʾҽ��ϴ�.");
        }
    }

    private void Update()
    {
        // �̵� ���̸� �̵� ���� ����
        if (isMoving)
        {
            MoveTargetObjectSmoothly();
        }

        // Ʈ���� ������Ʈ�� ���ų� �̺�Ʈ�� �̹� ����Ǿ��ٸ� ����
        if (triggerObject == null || eventTriggered) return;

        // Ʈ���� ������Ʈ�� �÷��̾� �浹 ����
        Collider[] colliders = Physics.OverlapBox(
            triggerObject.transform.position,
            triggerObject.GetComponent<Collider>().bounds.extents,
            triggerObject.transform.rotation
        );

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("�÷��̾�� Ʈ���� �浹 ����!");
                StartMovingTargetObject();
                eventTriggered = true; // �̺�Ʈ ���� �ߺ� ����
                break;
            }
        }
    }

    private void StartMovingTargetObject()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �̵� ���� ��ġ�� ��ǥ ��ġ ���
        startPosition = targetObject.transform.position;
        targetPosition = startPosition + targetObject.transform.forward.normalized * moveDistance;

        // �̵� ���� �÷��� Ȱ��ȭ
        isMoving = true;
    }

    private void MoveTargetObjectSmoothly()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �̵� ����: ���� ��ġ���� ��ǥ ��ġ�� �ε巴�� �̵�
        targetObject.transform.position = Vector3.MoveTowards(
            targetObject.transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // ��ǥ ��ġ�� �����ϸ� �̵� ����
        if (Vector3.Distance(targetObject.transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;
            Debug.Log("Ÿ�� ������Ʈ �̵� �Ϸ�");

            // Trigger Object ����
            if (triggerObject != null)
            {
                Destroy(triggerObject);
            }
        }
    }
}
