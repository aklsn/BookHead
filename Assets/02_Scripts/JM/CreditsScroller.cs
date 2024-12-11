using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    public RectTransform creditsText; // ũ���� �ؽ�Ʈ�� RectTransform
    public float scrollSpeed = 50f;   // ��ũ�� �ӵ�
    public float startDelay = 1f;     // ���� �� ��� �ð�

    private float startPositionY;     // �ʱ� Y ��ġ
    private float targetPositionY;    // ���� Y ��ġ

    public GameObject Cockroach;      // Cockroach ������Ʈ
    public float delay = 2f;          // Cockroach �̵� ���� �� ��� �ð�
    public float moveSpeed = 2f;      // �̵� �ӵ�
    public float moveDistance = 10f; // �̵� �Ÿ�

    private bool isMoving = false;    // �̵� �� ����
    private Vector3 cockroachStartPosition; // Cockroach�� ���� ��ġ
    private Vector3 cockroachTargetPosition; // Cockroach�� ��ǥ ��ġ

    private void Start()
    {
        if (creditsText == null)
        {
            Debug.LogError("Credits Text�� �������� �ʾҽ��ϴ�.");
            return;
        }

        if (Cockroach == null)
        {
            Debug.LogError("Cockroach ������Ʈ�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �ʱ� ��ġ �� ��ǥ ��ġ ����
        cockroachStartPosition = Cockroach.transform.position;
        cockroachTargetPosition = cockroachStartPosition - new Vector3(0, 0, moveDistance);

        // Cockroach �̵� ��� �� �̵� ����
        Invoke(nameof(StartCockroachMovement), delay);

        startPositionY = creditsText.anchoredPosition.y;
        targetPositionY = float.MaxValue;

        StartCoroutine(StartScrolling());
    }

    private IEnumerator StartScrolling()
    {
        yield return new WaitForSeconds(startDelay);

        while (creditsText.anchoredPosition.y < targetPositionY)
        {
            creditsText.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("ũ���� ��ũ�� �Ϸ�");
    }

    private void StartCockroachMovement()
    {
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveCockroach();
        }
    }

    private void MoveCockroach()
    {
        if (Cockroach == null) return;

        // Cockroach�� ��ǥ ��ġ�� �̵�
        Cockroach.transform.position = Vector3.MoveTowards(
            Cockroach.transform.position,
            cockroachTargetPosition,
            moveSpeed * Time.deltaTime
        );

        // ��ǥ ��ġ�� �����ϸ� �̵� ���� �� Cockroach ��ġ �ʱ�ȭ
        if (Vector3.Distance(Cockroach.transform.position, cockroachTargetPosition) < 0.01f)
        {
            isMoving = false;
            Cockroach.transform.position = cockroachStartPosition; // ��ġ �ʱ�ȭ
            Debug.Log("Cockroach �̵� �Ϸ�");
        }
    }
}
