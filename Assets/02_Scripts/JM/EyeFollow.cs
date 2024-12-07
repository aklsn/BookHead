using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public Transform leftEye; // ���� ������
    public Transform rightEye; // ������ ������

    public float eyeRange = 0.2f; // �������� �ִ� �̵� ����
    public float eyeSpeed = 5f; // �����ڰ� �̵��ϴ� �ӵ�
    private Vector3 leftEyeInitialPosition; // ���� ���� �ʱ� ��ġ
    private Vector3 rightEyeInitialPosition; // ������ ���� �ʱ� ��ġ

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform; // Transform �Ҵ�
        }
        else
        {
            Debug.LogWarning("Player �±׸� ���� ������Ʈ�� �������� �ʽ��ϴ�.");
        }

        // �� �������� �ʱ� ��ġ ����
        leftEyeInitialPosition = leftEye.localPosition;
        rightEyeInitialPosition = rightEye.localPosition;
    }

    void Update()
    {
        if (player == null) return;

        // �÷��̾��� ���� ��ǥ�� ���� ������Ʈ(����)�� ���� ��ǥ�� �������� ���� ���
        Vector3 directionToPlayer = player.position - transform.position;

        // ���� ���� �������� ��ȯ (������ �̵� ������ ���� �������� �����ϱ� ����)
        Vector3 localDirection = transform.InverseTransformDirection(directionToPlayer);

        // �̵� ������ ���� ���� ����
        float clampedX = Mathf.Clamp(localDirection.x, -eyeRange, eyeRange);
        float clampedY = Mathf.Clamp(localDirection.y, -eyeRange, eyeRange);

        // ���� ��ġ�� �������� �������� ��ǥ ��ġ ���
        Vector3 leftEyeTarget = new Vector3(
            leftEyeInitialPosition.x + clampedX,
            leftEyeInitialPosition.y, // Y�� ����
            leftEyeInitialPosition.z
        );

        Vector3 rightEyeTarget = new Vector3(
            rightEyeInitialPosition.x + clampedX,
            rightEyeInitialPosition.y, // Y�� ����
            rightEyeInitialPosition.z
        );

        // �ε巴�� ������ �̵� (Lerp ���)
        leftEye.localPosition = Vector3.Lerp(leftEye.localPosition, leftEyeTarget, eyeSpeed * Time.deltaTime);
        rightEye.localPosition = Vector3.Lerp(rightEye.localPosition, rightEyeTarget, eyeSpeed * Time.deltaTime);
    }
}
