using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EventCtrl_J : MonoBehaviour
{
    public GameObject triggerObject;       // ���� Ʈ���� Collider ������Ʈ
    public GameObject targetObject;        // ���� �̵��� ���� ������Ʈ
    public float moveSpeed = 10f;          // �̵� �ӵ�
    public float moveDistance = 50f;       // �̵� �Ÿ�

    public GameObject downTriggerObject;   // ���ο� Ʈ���� Collider ������Ʈ
    public GameObject downTargetObject;    // ���ο� �̵��� ���� ������Ʈ
    public float newMoveDistance = 10f;    // ���ο� �̵� �Ÿ�
    public float deactivateDelay = 2f;     // ��Ȱ��ȭ ��� �ð�

    public GameObject bedCamera;           // ħ�� �̺�Ʈ ī�޶�
    public GameObject bedEventObject;      // ħ�� �̺�Ʈ �� Ȱ��ȭ�� ������Ʈ
    public float bedEventDuration = 3f;    // ħ�� �̺�Ʈ ���� �ð�
    public Volume bedCameraVolume;         // Volume for bed camera
    public float fadeDuration = 2f;        // �� �ߴ� ȿ�� ���� �ð�

    private ColorAdjustments colorAdjustments;
    private bool eventTriggered = false;   // ���� �̺�Ʈ �ߺ� ���� ����
    private bool newEventTriggered = false; // ���ο� �̺�Ʈ �ߺ� ���� ����
    private bool bedEventTriggered = false; // ħ�� �̺�Ʈ �ߺ� ���� ����

    public string nextSceneName;

    private Vector3 startPosition;         // ���� �̵� ���� ��ġ
    private Vector3 targetPosition;        // ���� �̵� ��ǥ ��ġ
    private bool isMoving = false;         // ���� �̵� ������ ����

    private Vector3 newStartPosition;      // ���ο� �̵� ���� ��ġ
    private Vector3 newTargetPosition;     // ���ο� �̵� ��ǥ ��ġ
    private bool isNewMoving = false;      // ���ο� �̵� ������ ����

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

        if (downTriggerObject == null)
        {
            Debug.LogError("New Trigger Object�� �������� �ʾҽ��ϴ�.");
        }

        if (downTargetObject == null)
        {
            Debug.LogError("New Target Object�� �������� �ʾҽ��ϴ�.");
        }
        else
        {
            downTargetObject.SetActive(false); // �ʱ� ���� ��Ȱ��ȭ
        }

        if (bedCamera != null)
        {
            bedCamera.SetActive(false); // �ʱ� ���� ��Ȱ��ȭ
        }

        if (bedEventObject != null)
        {
            bedEventObject.SetActive(false); // �ʱ� ���� ��Ȱ��ȭ
        }

        if (bedCameraVolume != null && bedCameraVolume.profile.TryGet(out ColorAdjustments adjustments))
        {
            colorAdjustments = adjustments;
            colorAdjustments.postExposure.value = -10f; // �ʱ� ����
        }
    }

    private void Update()
    {
        // ���� �̵� ����
        if (isMoving)
        {
            MoveTargetObjectSmoothly();
        }

        if (triggerObject != null && !eventTriggered)
        {
            HandleTrigger(triggerObject, ref eventTriggered, StartMovingTargetObject);
        }

        // ���ο� �̵� ����
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
                Debug.Log($"�÷��̾�� {trigger.name} �浹 ����!");
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
            Debug.Log("���� Ÿ�� ������Ʈ �̵� �Ϸ�");

            // Trigger Object ����
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
            Debug.LogError("New Target Object�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ���ο� �̵� ���� ��ġ�� ��ǥ ��ġ ���
        downTargetObject.SetActive(true); // ��Ȱ��ȭ ���¿��� Ȱ��ȭ
        newStartPosition = downTargetObject.transform.position;
        newTargetPosition = newStartPosition - new Vector3(0, newMoveDistance, 0); // Y�� �������� �̵�

        // �̵� ���� �÷��� Ȱ��ȭ
        isNewMoving = true;
    }

    private void MoveNewTargetObjectSmoothly()
    {
        float downSpeed = 0.35f;
        if (downTargetObject == null)
        {
            Debug.LogError("New Target Object�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �̵� ����: ���� ��ġ���� ��ǥ ��ġ�� �ε巴�� �̵�
        downTargetObject.transform.position = Vector3.MoveTowards(
            downTargetObject.transform.position,
            newTargetPosition,
            downSpeed * Time.deltaTime
        );

        // ��ǥ ��ġ�� �����ϸ� �̵� ����
        if (Vector3.Distance(downTargetObject.transform.position, newTargetPosition) < 0.01f)
        {
            isNewMoving = false;
            Debug.Log("���ο� Ÿ�� ������Ʈ �̵� �Ϸ�");

            // ���� �ð� �� ��Ȱ��ȭ
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
        if (bedEventTriggered) return; // �̺�Ʈ �ߺ� ����
        bedEventTriggered = true;

        // Fog Ȱ��ȭ �� ����
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.3f;

        if (bedCamera != null)
        {
            bedCamera.SetActive(true); // ī�޶� Ȱ��ȭ
            StartCoroutine(FadeExposureToZero());
            Invoke(nameof(EndBedEvent), bedEventDuration); // ī�޶� ��Ȱ��ȭ ����
        }

        if (bedEventObject != null)
        {
            bedEventObject.SetActive(true); // �̺�Ʈ ������Ʈ Ȱ��ȭ
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
                rb.isKinematic = false; // Rigidbody Ȱ��ȭ
                rb.useGravity = true;   // �߷� Ȱ��ȭ
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

        obj.transform.rotation = targetRotation; // ����
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

        colorAdjustments.postExposure.value = 0f; // ����
    }

    private void EndBedEvent()
    {
        if (bedCamera != null)
        {
            bedCamera.SetActive(false); // ī�޶� ��Ȱ��ȭ
        }

        RenderSettings.fog = false; // Fog ��Ȱ��ȭ

        // �� ��ȯ
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("���� �� �̸��� �������� �ʾҽ��ϴ�.");
        }

        bedEventTriggered = false; // �̺�Ʈ �ʱ�ȭ ����
    }
}
