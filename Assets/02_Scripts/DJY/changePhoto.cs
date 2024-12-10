using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changePhoto : MonoBehaviour
{
    [Header("Frame Settings")]
    public GameObject frameObject; // 액자 오브젝트
    public Material newMaterial;   // 변경할 메테리얼

    [Header("Door Settings")]
    public doorController targetDoor; // 대상 문 컨트롤러

    private bool materialChanged = false; // 메테리얼 변경 상태 확인

    void Update()
    {
        if (targetDoor != null && targetDoor.open && !materialChanged)
        {
            StartCoroutine(ChangeFrameMaterialWithDelay(2f)); // 2초 지연 후 메테리얼 변경
        }
    }

    private IEnumerator ChangeFrameMaterialWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeFrameMaterial();
    }

    public void ChangeFrameMaterial()
    {
        if (!materialChanged && frameObject != null && newMaterial != null)
        {
            Renderer frameRenderer = frameObject.GetComponent<Renderer>();
            if (frameRenderer != null)
            {
                frameRenderer.material = newMaterial;
                materialChanged = true;
                Debug.Log("액자의 이미지가 변경되었습니다.");
            }
        }
    }
}
