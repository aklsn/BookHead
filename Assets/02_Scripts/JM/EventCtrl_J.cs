using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameObject triggerObject;   // Trigger Collider 오브젝트
    public GameObject targetObject;    // 이동할 게임 오브젝트
    public float moveSpeed = 10f;      // 이동 속도
    public float moveDistance = 50f;  // 이동 거리

    private bool eventTriggered = false; // 이벤트 중복 실행 방지
    private Vector3 startPosition;       // 이동 시작 위치
    private Vector3 targetPosition;      // 이동 목표 위치
    private bool isMoving = false;       // 이동 중인지 여부

    private void Start()
    {
        if (triggerObject == null)
        {
            Debug.LogError("Trigger Object가 설정되지 않았습니다.");
        }

        if (targetObject == null)
        {
            Debug.LogError("Target Object가 설정되지 않았습니다.");
        }
    }

    private void Update()
    {
        // 이동 중이면 이동 로직 실행
        if (isMoving)
        {
            MoveTargetObjectSmoothly();
        }

        // 트리거 오브젝트가 없거나 이벤트가 이미 실행되었다면 종료
        if (triggerObject == null || eventTriggered) return;

        // 트리거 오브젝트와 플레이어 충돌 감지
        Collider[] colliders = Physics.OverlapBox(
            triggerObject.transform.position,
            triggerObject.GetComponent<Collider>().bounds.extents,
            triggerObject.transform.rotation
        );

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("플레이어와 트리거 충돌 감지!");
                StartMovingTargetObject();
                eventTriggered = true; // 이벤트 실행 중복 방지
                break;
            }
        }
    }

    private void StartMovingTargetObject()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object가 설정되지 않았습니다.");
            return;
        }

        // 이동 시작 위치와 목표 위치 계산
        startPosition = targetObject.transform.position;
        targetPosition = startPosition + targetObject.transform.forward.normalized * moveDistance;

        // 이동 시작 플래그 활성화
        isMoving = true;
    }

    private void MoveTargetObjectSmoothly()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object가 설정되지 않았습니다.");
            return;
        }

        // 이동 로직: 현재 위치에서 목표 위치로 부드럽게 이동
        targetObject.transform.position = Vector3.MoveTowards(
            targetObject.transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // 목표 위치에 도달하면 이동 중지
        if (Vector3.Distance(targetObject.transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;
            Debug.Log("타겟 오브젝트 이동 완료");

            // Trigger Object 삭제
            if (triggerObject != null)
            {
                Destroy(triggerObject);
            }
        }
    }
}
