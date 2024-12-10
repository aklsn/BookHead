using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPTrigger : MonoBehaviour
{
    public doorController doorController;
    public GhostManager ghostManager;
    public AudioSource sound1; // 연결된 
    public Transform player;    //Player위치 정보값
    public bool IsLp; // 사운드 재생 상태 플래그
    public bool IsDone;
    public float minDistance = 1f; // 최소 거리
    public float maxDistance = 20f; // 최대 거리
    private float minPitch = 1f; // 최소 피치 값
    private float maxPitch = 2f; // 최대 피치 값

    private void Start()
    {
        IsLp = false; // 초기값 설정
        IsDone = false;
    }

    private void Update()
    {
        if (!IsLp)
        {
            sound1.Stop();
        }
        else
        {
            // Player와 이 오브젝트 사이의 거리 계산
            float distance = Vector3.Distance(player.position, transform.position);

            // 거리를 0~1 사이로 정규화
            float t = Mathf.InverseLerp(maxDistance, minDistance, distance);

            // 정규화된 값을 기반으로 피치 계산
            float pitch = Mathf.Lerp(minPitch, maxPitch, t);

            // AudioSource의 피치 값을 업데이트
            sound1.pitch = pitch; 
        }

        if (IsDone)
        {
            ghostManager.ghost1.SetActive(true);
            IsDone = false;
        }
    }
}