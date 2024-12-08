using System.Collections;
using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [Header("Light Settings")]
    public Light flashlight; // 손전등 Light 컴포넌트
    public float blinkInterval = 0.5f; // 깜빡이는 간격 (초)
    public bool isBlinking = false; // 깜빡임 활성화 여부

    void Start()
    {
        if (flashlight == null)
        {
            flashlight = GetComponent<Light>();
        }
    }

    void Update()
    {
        // 키 입력으로 깜빡임 토글 (예: F 키로 깜빡임 활성화/비활성화)
        if (Input.GetKeyDown(KeyCode.F))
        {
            isBlinking = !isBlinking;
            if (isBlinking)
            {
                StartCoroutine(BlinkLight());
            }
            else
            {
                StopCoroutine(BlinkLight());
                flashlight.enabled = true; // 깜빡임 멈춘 후 기본 상태 켜짐
            }
        }
    }

    IEnumerator BlinkLight()
    {
        while (isBlinking)
        {
            flashlight.enabled = !flashlight.enabled; // 켜고 끄기 토글
            yield return new WaitForSeconds(blinkInterval); // 설정된 간격 대기
        }
    }
}
