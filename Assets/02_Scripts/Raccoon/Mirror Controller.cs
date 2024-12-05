using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Mirror : MonoBehaviour
{
    public Camera mirrorCamera; // 거울 반사를 위한 카메라
    private RenderTexture renderTexture; // 고유 RenderTexture
    private Renderer mirrorRenderer;

    void Awake()
    {
        mirrorRenderer = GetComponent<Renderer>();
        renderTexture = CreateUniqueRenderTexture();

        if (mirrorCamera != null)
        {
            mirrorCamera.targetTexture = renderTexture;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: mirrorCamera가 설정되지 않았습니다");
        }

        ApplyRenderTextureToMaterial();
    }

    void LateUpdate()
    {
        if (mirrorCamera != null)
        {
            UpdateCameraTransform();
        }
    }

    private RenderTexture CreateUniqueRenderTexture()
    {
        RenderTexture rt = new RenderTexture(1024, 1920, 16, RenderTextureFormat.Default);
        rt.name = $"{gameObject.name}_RenderTexture"; 
        return rt;
    }

    private void ApplyRenderTextureToMaterial()
    {
        if (mirrorRenderer != null)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetTexture("_BaseMap", renderTexture);
            mirrorRenderer.SetPropertyBlock(block);
        }
    }

    private void UpdateCameraTransform()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector3 mirrorNormal = transform.forward;
        Vector3 mirrorPosition = transform.position;

        Vector3 reflectedPosition = mainCamera.transform.position -
                                     2 * Vector3.Dot(mainCamera.transform.position - mirrorPosition, mirrorNormal) * mirrorNormal;

        Vector3 reflectedDirection = Vector3.Reflect(mainCamera.transform.forward, mirrorNormal);

        //mirrorCamera.transform.position = reflectedPosition; // 위치 잡기 귀찮아서 그냥 주석처리함 !! 혹시라도 오류 뜨거나 이상 있으면 말해 줄 것
        mirrorCamera.transform.rotation = Quaternion.LookRotation(reflectedDirection, Vector3.up);
    }
}
