using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toiletWalk : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource; // 재생할 AudioSource
    public AudioClip triggerClip;   // 트리거를 밟으면 재생할 오디오 클립


    public LockableDoorManager1 doorManager;

    [Header("Trigger Settings")]
    public string playerTag = "Player"; // 트리거를 밟을 오브젝트의 태그

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 트리거에 진입했는지 확인
        if (other.CompareTag(playerTag))
        {
            PlayAudio();
        }
    }

    private void PlayAudio()
    {
        if (audioSource != null && triggerClip != null)
        {
            audioSource.clip = triggerClip; // 오디오 클립 설정
            audioSource.Play(); // 오디오 재생
        }

    }
}
