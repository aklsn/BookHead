using System.Collections;
using UnityEngine;

public class MannequinLightController : MonoBehaviour
{
    public GameObject mannequin; // ����ŷ ������Ʈ
    public Light sceneLight;     // �� ������Ʈ
    public float lightBlinkDuration = 0.1f; // �� �����̴� ����
    public int blinkCount = 2;   // �� �����̴� Ƚ��
    public float lightOnDuration = 1f; // ���� ���� �ִ� �ð�
    public float mannequinOffDuration = 5f; // ����ŷ ��Ȱ��ȭ ���� �ð�

    private bool isMannequinVisible = false; // ����ŷ ����

    private void Start()
    {
        if (mannequin == null || sceneLight == null)
        {
            Debug.LogError("Mannequin �Ǵ� Light�� �������� �ʾҽ��ϴ�.");
            return;
        }

        mannequin.SetActive(false); // �ʱ� ���´� ��Ȱ��ȭ
        StartCoroutine(LoopMannequinLight());
    }

    private IEnumerator LoopMannequinLight()
    {
        while (true)
        {
            // 1. �� ������
            yield return StartCoroutine(BlinkLight());

            // 2. ����ŷ ���� ��ȯ
            ToggleMannequin();

            // 3. ���¿� ���� �ٸ� ���� �ð�
            if (isMannequinVisible)
            {
                // ����ŷ Ȱ��ȭ�� ���¿��� �� ���� ����
                yield return new WaitForSeconds(lightOnDuration);
            }
            else
            {
                // ����ŷ ��Ȱ��ȭ ���� ����
                yield return new WaitForSeconds(mannequinOffDuration);
            }
        }
    }

    private IEnumerator BlinkLight()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            sceneLight.enabled = false; // �� ����
            yield return new WaitForSeconds(lightBlinkDuration);
            sceneLight.enabled = true; // �� �ѱ�
            yield return new WaitForSeconds(lightBlinkDuration);
        }
    }

    private void ToggleMannequin()
    {
        isMannequinVisible = !isMannequinVisible; // ���� ���
        mannequin.SetActive(isMannequinVisible); // ����ŷ Ȱ��ȭ/��Ȱ��ȭ
        Debug.Log($"����ŷ ����: {(isMannequinVisible ? "Ȱ��ȭ" : "��Ȱ��ȭ")}");
    }
}
