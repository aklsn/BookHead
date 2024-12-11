using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    public RectTransform creditsText; // 크레딧 텍스트의 RectTransform
    public float scrollSpeed = 50f;   // 스크롤 속도
    public float startDelay = 1f;     // 시작 전 대기 시간

    private float startPositionY;     // 초기 Y 위치
    private float targetPositionY;    // 최종 Y 위치

    public GameObject Cockroach;      // Cockroach 오브젝트
    public float delay = 2f;          // Cockroach 이동 시작 전 대기 시간
    public float moveSpeed = 2f;      // 이동 속도
    public float moveDistance = 10f; // 이동 거리

    private bool isMoving = false;    // 이동 중 여부
    private Vector3 cockroachStartPosition; // Cockroach의 시작 위치
    private Vector3 cockroachTargetPosition; // Cockroach의 목표 위치

    private void Start()
    {
        if (creditsText == null)
        {
            Debug.LogError("Credits Text가 설정되지 않았습니다.");
            return;
        }

        if (Cockroach == null)
        {
            Debug.LogError("Cockroach 오브젝트가 설정되지 않았습니다.");
            return;
        }

        // 초기 위치 및 목표 위치 설정
        cockroachStartPosition = Cockroach.transform.position;
        cockroachTargetPosition = cockroachStartPosition - new Vector3(0, 0, moveDistance);

        // Cockroach 이동 대기 후 이동 시작
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

        Debug.Log("크레딧 스크롤 완료");
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

        // Cockroach를 목표 위치로 이동
        Cockroach.transform.position = Vector3.MoveTowards(
            Cockroach.transform.position,
            cockroachTargetPosition,
            moveSpeed * Time.deltaTime
        );

        // 목표 위치에 도달하면 이동 중지 및 Cockroach 위치 초기화
        if (Vector3.Distance(Cockroach.transform.position, cockroachTargetPosition) < 0.01f)
        {
            isMoving = false;
            Cockroach.transform.position = cockroachStartPosition; // 위치 초기화
            Debug.Log("Cockroach 이동 완료");
        }
    }
}
