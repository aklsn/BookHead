using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource; // 재생할 AudioSource
    public AudioClip firstClip;     // 첫 번째 재생할 오디오 클립
    public AudioClip secondClip;    // 두 번째 재생할 오디오 클립

    [Header("Trigger Settings")]
    public string playerTag = "Player"; // 트리거를 밟을 오브젝트의 태그

    private bool isTriggered = false; // 트리거가 이미 실행되었는지 확인

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 트리거에 진입했는지 확인
        if (other.CompareTag(playerTag) && !isTriggered)
        {
            isTriggered = true; // 한 번만 실행
            StartCoroutine(PlayAudioSequence());
        }
    }

    private IEnumerator PlayAudioSequence()
    {
        if (audioSource != null)
        {
            // 첫 번째 클립 재생
            if (firstClip != null)
            {
                audioSource.clip = firstClip;
                audioSource.Play();
                Debug.Log("첫 번째 오디오 클립 재생: " + firstClip.name);
                yield return new WaitForSeconds(firstClip.length); // 첫 번째 클립 길이만큼 대기
            }

            // 두 번째 클립 재생
            if (secondClip != null)
            {
                audioSource.clip = secondClip;
                audioSource.Play();
                Debug.Log("두 번째 오디오 클립 재생: " + secondClip.name);
                yield return new WaitForSeconds(secondClip.length); // 두 번째 클립 길이만큼 대기
            }
        }
        else
        {
            Debug.LogError("AudioSource가 설정되지 않았습니다.");
        }
    }
}
