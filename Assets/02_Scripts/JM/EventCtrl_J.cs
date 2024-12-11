using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EventCtrl_J : MonoBehaviour
{
    public GameObject triggerObject;       // 기존 트리거 Collider 오브젝트
    public GameObject targetObject;        // 기존 이동할 게임 오브젝트
    public float moveSpeed = 10f;          // 이동 속도
    public float moveDistance = 50f;       // 이동 거리

    public GameObject downTriggerObject;   // 새로운 트리거 Collider 오브젝트
    public GameObject downTargetObject;    // 새로운 이동할 게임 오브젝트
    public float newMoveDistance = 10f;    // 새로운 이동 거리
    public float deactivateDelay = 2f;     // 비활성화 대기 시간

    public GameObject bedCamera;           // 침대 이벤트 카메라
    public GameObject bedEventObject;      // 침대 이벤트 시 활성화할 오브젝트
    public float bedEventDuration = 3f;    // 침대 이벤트 지속 시간
    public Volume bedCameraVolume;         // Volume for bed camera
    public float fadeDuration = 2f;        // 눈 뜨는 효과 지속 시간

    private ColorAdjustments colorAdjustments;
    private bool eventTriggered = false;   // 기존 이벤트 중복 실행 방지
    private bool newEventTriggered = false; // 새로운 이벤트 중복 실행 방지
    private bool bedEventTriggered = false; // 침대 이벤트 중복 실행 방지

    public string nextSceneName;

    private Vector3 startPosition;         // 기존 이동 시작 위치
    private Vector3 targetPosition;        // 기존 이동 목표 위치
    private bool isMoving = false;         // 기존 이동 중인지 여부

    private Vector3 newStartPosition;      // 새로운 이동 시작 위치
    private Vector3 newTargetPosition;     // 새로운 이동 목표 위치
    private bool isNewMoving = false;      // 새로운 이동 중인지 여부

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

        if (downTriggerObject == null)
        {
            Debug.LogError("New Trigger Object가 설정되지 않았습니다.");
        }

        if (downTargetObject == null)
        {
            Debug.LogError("New Target Object가 설정되지 않았습니다.");
        }
        else
        {
            downTargetObject.SetActive(false); // 초기 상태 비활성화
        }

        if (bedCamera != null)
        {
            bedCamera.SetActive(false); // 초기 상태 비활성화
        }

        if (bedEventObject != null)
        {
            bedEventObject.SetActive(false); // 초기 상태 비활성화
        }

        if (bedCameraVolume != null && bedCameraVolume.profile.TryGet(out ColorAdjustments adjustments))
        {
            colorAdjustments = adjustments;
            colorAdjustments.postExposure.value = -10f; // 초기 상태
        }
    }

    private void Update()
    {
        // 기존 이동 로직
        if (isMoving)
        {
            MoveTargetObjectSmoothly();
        }

        if (triggerObject != null && !eventTriggered)
        {
            HandleTrigger(triggerObject, ref eventTriggered, StartMovingTargetObject);
        }

        // 새로운 이동 로직
        if (isNewMoving)
        {
            MoveNewTargetObjectSmoothly();
        }

        if (downTriggerObject != null && !newEventTriggered)
        {
            HandleTrigger(downTriggerObject, ref newEventTriggered, StartMovingNewTargetObject);
        }
    }

    private void HandleTrigger(GameObject trigger, ref bool triggered, System.Action action)
    {
        Collider[] colliders = Physics.OverlapBox(
            trigger.transform.position,
            trigger.GetComponent<Collider>().bounds.extents,
            trigger.transform.rotation
        );

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log($"플레이어와 {trigger.name} 충돌 감지!");
                action.Invoke();
                triggered = true;
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
            Debug.Log("기존 타겟 오브젝트 이동 완료");

            // Trigger Object 삭제
            if (triggerObject != null)
            {
                Destroy(triggerObject);
            }
        }
    }

    private void StartMovingNewTargetObject()
    {
        if (downTargetObject == null)
        {
            Debug.LogError("New Target Object가 설정되지 않았습니다.");
            return;
        }

        // 새로운 이동 시작 위치와 목표 위치 계산
        downTargetObject.SetActive(true); // 비활성화 상태에서 활성화
        newStartPosition = downTargetObject.transform.position;
        newTargetPosition = newStartPosition - new Vector3(0, newMoveDistance, 0); // Y축 방향으로 이동

        // 이동 시작 플래그 활성화
        isNewMoving = true;
    }

    private void MoveNewTargetObjectSmoothly()
    {
        float downSpeed = 0.35f;
        if (downTargetObject == null)
        {
            Debug.LogError("New Target Object가 설정되지 않았습니다.");
            return;
        }

        // 이동 로직: 현재 위치에서 목표 위치로 부드럽게 이동
        downTargetObject.transform.position = Vector3.MoveTowards(
            downTargetObject.transform.position,
            newTargetPosition,
            downSpeed * Time.deltaTime
        );

        // 목표 위치에 도달하면 이동 중지
        if (Vector3.Distance(downTargetObject.transform.position, newTargetPosition) < 0.01f)
        {
            isNewMoving = false;
            Debug.Log("새로운 타겟 오브젝트 이동 완료");

            // 일정 시간 후 비활성화
            StartCoroutine(DeactivateDownTargetObject());
        }
    }

    private IEnumerator DeactivateDownTargetObject()
    {
        yield return new WaitForSeconds(deactivateDelay);
        downTargetObject.SetActive(false);
    }

    public void StartBedEvent()
    {
        if (bedEventTriggered) return; // 이벤트 중복 방지
        bedEventTriggered = true;

        // Fog 활성화 및 설정
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.3f;

        if (bedCamera != null)
        {
            bedCamera.SetActive(true); // 카메라 활성화
            StartCoroutine(FadeExposureToZero());
            Invoke(nameof(EndBedEvent), bedEventDuration); // 카메라 비활성화 예약
        }

        if (bedEventObject != null)
        {
            bedEventObject.SetActive(true); // 이벤트 오브젝트 활성화
            Invoke(nameof(ActivateRigidbody), 2.3f);
        }
    }

    private void ActivateRigidbody()
    {
        if (bedEventObject != null)
        {
            Rigidbody rb = bedEventObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Rigidbody 활성화
                rb.useGravity = true;   // 중력 활성화
            }

            StartCoroutine(RotateObject(bedEventObject, Quaternion.Euler(0, 0, 0), 0.3f));
        }
    }

    private IEnumerator RotateObject(GameObject obj, Quaternion targetRotation, float duration)
    {
        if (obj == null) yield break;

        Quaternion initialRotation = obj.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            obj.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / duration);
            yield return null;
        }

        obj.transform.rotation = targetRotation; // 보정
    }

    private IEnumerator FadeExposureToZero()
    {
        float elapsedTime = 0f;
        float initialValue = colorAdjustments.postExposure.value;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            colorAdjustments.postExposure.value = Mathf.Lerp(initialValue, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        colorAdjustments.postExposure.value = 0f; // 보정
    }

    private void EndBedEvent()
    {
        if (bedCamera != null)
        {
            bedCamera.SetActive(false); // 카메라 비활성화
        }

        RenderSettings.fog = false; // Fog 비활성화

        // 씬 전환
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("다음 씬 이름이 설정되지 않았습니다.");
        }

        bedEventTriggered = false; // 이벤트 초기화 가능
    }
}
