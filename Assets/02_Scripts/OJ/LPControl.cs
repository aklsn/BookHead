using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPControl : MonoBehaviour
{
    public AudioSource sound1; // 첫 번째 소리
    public AudioSource sound2; // 두 번째 소리 (옵션)
    public bool isLP; // LP 동작 여부를 나타내는 플래그

    private void OnTriggerEnter(Collider other)
    {
        // Player가 이 오브젝트와 충돌했을 때
        if (other.CompareTag("Player"))
        {
            if (sound1 != null && !sound1.isPlaying)
            {
                sound1.Play();
                isLP = true;
            }
        }
    }
}