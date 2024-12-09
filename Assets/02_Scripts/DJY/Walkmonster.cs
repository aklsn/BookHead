using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkmonster : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource; // 발소리 및 문 여는 소리 재생용 오디오 소스
    public AudioClip walkingSound; // 걸어가는 소리
    public AudioClip doorOpeningSound; // 문 여는 소리
    public AudioClip doorLockingSound; // 문 잠그는 소리

    [Header("Door Settings")]
    public doorController targetDoor; // 몬스터가 열 문

    [Header("Timing Settings")]
    public float walkingDuration = 5f; // 걸어가는 소리 지속 시간
    public float doorCloseDelay = 1f; // 문 닫기 전 지연 시간
    public float doorLockDuration = 3f; // 문 잠금 지속 시간

    private bool isTriggered = false; // 트리거 상태 확인

    void OnTriggerEnter(Collider other)
    {
        // 트리거 발동 시 효과 시작
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(PlayMonsterEffects());
        }
    }

    private IEnumerator PlayMonsterEffects()
    {
        // 문 닫기 및 잠금 처리
        if (targetDoor != null)
        {
            yield return new WaitForSeconds(doorCloseDelay);
            targetDoor.ChangeDoorState(); // 문 닫기
            targetDoor.CloseControl = true; // 문 잠금 설정
            Debug.Log("문이 닫히고 잠겼습니다.");
        }

        // 걸어가는 소리 재생
        if (audioSource != null && walkingSound != null)
        {
            audioSource.clip = walkingSound;
            audioSource.loop = true;
            audioSource.Play();
            Debug.Log("몬스터 걸어가는 소리 재생 중...");
        }

        // 걸어가는 소리 지속 시간 대기
        yield return new WaitForSeconds(walkingDuration);

        // 걸어가는 소리 중지
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
            Debug.Log("몬스터 걸어가는 소리 중지.");
        }

        // 문 열기 및 잠금 해제 처리
        if (targetDoor != null)
        {
            if (audioSource != null && doorOpeningSound != null)
            {
                audioSource.clip = doorOpeningSound;
                audioSource.loop = false;
                audioSource.Play();
                Debug.Log("몬스터가 문 여는 소리 재생 중...");
            }

            yield return new WaitForSeconds(doorLockDuration);
            targetDoor.CloseControl = false; // 문 잠금 해제
            targetDoor.ChangeDoorState(); // 문 열기
            Debug.Log("문 잠금이 해제되고 열렸습니다.");
        }
    }
}
