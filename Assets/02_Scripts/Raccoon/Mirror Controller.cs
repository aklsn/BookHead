using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Mirror : MonoBehaviour
{
    public Camera mirrorCamera; // �ſ� �ݻ縦 ���� ī�޶�
    private RenderTexture renderTexture; // ���� RenderTexture
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
            Debug.LogWarning($"{gameObject.name}: mirrorCamera�� �������� �ʾҽ��ϴ�");
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

        //mirrorCamera.transform.position = reflectedPosition; // ��ġ ��� �����Ƽ� �׳� �ּ�ó���� !! Ȥ�ö� ���� �߰ų� �̻� ������ ���� �� ��
        mirrorCamera.transform.rotation = Quaternion.LookRotation(reflectedDirection, Vector3.up);
    }
}
