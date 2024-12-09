using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockableDoorManager : MonoBehaviour
{
    [Header("Door Settings")]
    public doorController controlledDoor; // 기존 doorController 연결

    [Header("Trigger Settings")]
    public Collider lockTrigger; // 잠금 트리거

    private bool isLocked = false; // 문 잠금 상태

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other == lockTrigger)
        {
            Debug.Log("플레이어가 잠금 트리거를 밟았습니다.");
            LockDoor();
        }
    }
    
    public void LockDoor()
    {
        if (controlledDoor != null)
        {
            controlledDoor.CloseDoor(); // 문 닫기 메서드 호출
            Debug.Log("문이 닫히고 잠겼습니다.");
            isLocked = true; // 문 잠금 설정
        }
    }

    public void UnlockDoor()
    {
        if (isLocked)
        {
            isLocked = false;
            Debug.Log("문 잠금이 해제되었습니다.");
        }
    }
}
