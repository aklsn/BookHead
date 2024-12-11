using System.Collections;
using UnityEngine;

public class MannequinLightController : MonoBehaviour
{
    public GameObject mannequin; // 마네킹 오브젝트
    public Light sceneLight;     // 빛 오브젝트
    public float lightBlinkDuration = 0.1f; // 빛 깜빡이는 간격
    public int blinkCount = 2;   // 빛 깜빡이는 횟수
    public float lightOnDuration = 1f; // 빛이 켜져 있는 시간
    public float mannequinOffDuration = 5f; // 마네킹 비활성화 유지 시간

    private bool isMannequinVisible = false; // 마네킹 상태

    private void Start()
    {
        if (mannequin == null || sceneLight == null)
        {
            Debug.LogError("Mannequin 또는 Light가 설정되지 않았습니다.");
            return;
        }

        mannequin.SetActive(false); // 초기 상태는 비활성화
        StartCoroutine(LoopMannequinLight());
    }

    private IEnumerator LoopMannequinLight()
    {
        while (true)
        {
            // 1. 빛 깜빡임
            yield return StartCoroutine(BlinkLight());

            // 2. 마네킹 상태 전환
            ToggleMannequin();

            // 3. 상태에 따라 다른 유지 시간
            if (isMannequinVisible)
            {
                // 마네킹 활성화된 상태에서 빛 켜짐 유지
                yield return new WaitForSeconds(lightOnDuration);
            }
            else
            {
                // 마네킹 비활성화 상태 유지
                yield return new WaitForSeconds(mannequinOffDuration);
            }
        }
    }

    private IEnumerator BlinkLight()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            sceneLight.enabled = false; // 빛 끄기
            yield return new WaitForSeconds(lightBlinkDuration);
            sceneLight.enabled = true; // 빛 켜기
            yield return new WaitForSeconds(lightBlinkDuration);
        }
    }

    private void ToggleMannequin()
    {
        isMannequinVisible = !isMannequinVisible; // 상태 토글
        mannequin.SetActive(isMannequinVisible); // 마네킹 활성화/비활성화
        Debug.Log($"마네킹 상태: {(isMannequinVisible ? "활성화" : "비활성화")}");
    }
}
