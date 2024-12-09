using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class soundPlay : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip audioClip; // 재생할 오디오 파일
    public float triggerRadius = 1f; // 트리거 반경

    [Header("Target Point")]
    public Transform targetPoint; // 목표 위치

    [Header("Door Manager")]
    public LockableDoorManager doorManager; 

    private AudioSource audioSource;
    private bool audioPlayed = false; // 오디오가 이미 재생되었는지 확인

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
        }
    }

    void Update()
    {
        if (targetPoint != null && !audioPlayed)
        {
            float distance = Vector3.Distance(transform.position, targetPoint.position);
            if (distance <= triggerRadius)
            {
                PlayAudio();
            }
        }
    }

    private void PlayAudio()
    {
        if (audioSource != null && !audioPlayed)
        {
            audioSource.Play();
            audioPlayed = true;
            Debug.Log("오디오 재생됨: " + audioClip.name);
            StartCoroutine(UnlockDoorAfterSound());
        }
    }

        private IEnumerator UnlockDoorAfterSound()
    {
        while (audioSource.isPlaying)
        {
            yield return null; // 오디오 재생이 끝날 때까지 대기
        }

        if (doorManager != null)
        {
            doorManager.UnlockDoor(); // 문 잠금 해제
        }
    }

    void OnDrawGizmosSelected()
    {
        if (targetPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(targetPoint.position, triggerRadius);
        }
    }
}
