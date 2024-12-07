using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public Transform leftEye; // 왼쪽 눈동자
    public Transform rightEye; // 오른쪽 눈동자

    public float eyeRange = 0.2f; // 눈동자의 최대 이동 범위
    public float eyeSpeed = 5f; // 눈동자가 이동하는 속도
    private Vector3 leftEyeInitialPosition; // 왼쪽 눈의 초기 위치
    private Vector3 rightEyeInitialPosition; // 오른쪽 눈의 초기 위치

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform; // Transform 할당
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트가 존재하지 않습니다.");
        }

        // 각 눈동자의 초기 위치 저장
        leftEyeInitialPosition = leftEye.localPosition;
        rightEyeInitialPosition = rightEye.localPosition;
    }

    void Update()
    {
        if (player == null) return;

        // 플레이어의 월드 좌표와 현재 오브젝트(액자)의 월드 좌표를 기준으로 방향 계산
        Vector3 directionToPlayer = player.position - transform.position;

        // 액자 로컬 공간으로 변환 (눈동자 이동 제한을 로컬 기준으로 설정하기 위함)
        Vector3 localDirection = transform.InverseTransformDirection(directionToPlayer);

        // 이동 가능한 범위 내로 제한
        float clampedX = Mathf.Clamp(localDirection.x, -eyeRange, eyeRange);
        float clampedY = Mathf.Clamp(localDirection.y, -eyeRange, eyeRange);

        // 기존 위치를 기준으로 눈동자의 목표 위치 계산
        Vector3 leftEyeTarget = new Vector3(
            leftEyeInitialPosition.x + clampedX,
            leftEyeInitialPosition.y, // Y는 고정
            leftEyeInitialPosition.z
        );

        Vector3 rightEyeTarget = new Vector3(
            rightEyeInitialPosition.x + clampedX,
            rightEyeInitialPosition.y, // Y는 고정
            rightEyeInitialPosition.z
        );

        // 부드럽게 눈동자 이동 (Lerp 사용)
        leftEye.localPosition = Vector3.Lerp(leftEye.localPosition, leftEyeTarget, eyeSpeed * Time.deltaTime);
        rightEye.localPosition = Vector3.Lerp(rightEye.localPosition, rightEyeTarget, eyeSpeed * Time.deltaTime);
    }
}
