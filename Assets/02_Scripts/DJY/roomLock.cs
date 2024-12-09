using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomLock : MonoBehaviour
{
    [Header("Door Settings")]
    public doorController targetDoor; // 잠금을 풀 문

    [Header("Locked Room Settings")]
    public doorController lockedRoomDoor; // 잠긴 방의 문
    public AudioSource LockDoor;
    public AudioClip openLockDoor;

    private bool isRoomLocked = true; // 방 잠금 상태


    void Update()
    {
        if (targetDoor != null && isRoomLocked)
        {
            // 문이 열렸는지 확인
            if (targetDoor.open)
            {
                UnlockRoom();
            }
        }
    }

    public void LockRoom()
    {
        if (lockedRoomDoor != null)
        {
            isRoomLocked = true;
            lockedRoomDoor.CloseControl = true; // 잠긴 방의 문 닫기 및 잠금
            Debug.Log("방이 잠겼습니다.");
        }
    }

    public void UnlockRoom()
    {
        if (lockedRoomDoor != null)
        {
            LockDoor.clip = openLockDoor;
            LockDoor.Play();
            isRoomLocked = false;
            lockedRoomDoor.CloseControl = false; // 잠금 해제
            lockedRoomDoor.open = true; // 잠긴 방의 문 열기
            Debug.Log("방이 열렸습니다.");
        }
    }
}