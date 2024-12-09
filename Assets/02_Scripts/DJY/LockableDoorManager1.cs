using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockableDoorManager1 : MonoBehaviour
{
    [Header("Door Settings")]
    public doorController controlledDoor; // 기존 doorController 연결

    [Header("Trigger Settings")]
    public GameObject lockTrigger; // 잠금 트리거


    public void LockDoor()
    {
        if (controlledDoor != null && controlledDoor.open)
        {
            controlledDoor.CloseDoor(); // 문 닫기
            controlledDoor.CloseControl = true;
        }
        
    }

    public void UnlockDoor()
    {
        controlledDoor.CloseControl = false;
    }
}