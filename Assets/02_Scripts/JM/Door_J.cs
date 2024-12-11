using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_J : MonoBehaviour
{
    [Header("Door Settings")]
    public doorController door1; // 첫 번째 문
    public doorController door2; // 두 번째 문
    public doorController door3; // 세 번째 문

    private bool door1Unlocked = false; // 문 1 잠금 해제 상태
    private bool door2Unlocked = false; // 문 2 잠금 해제 상태
    private bool door3Unlocked = false; // 문 3 잠금 해제 상태

    private void Update()
    {
        // 문 2 잠금 해제: 문 1이 열렸을 때
        if (door1.open && !door2Unlocked)
        {
            UnlockDoor(door2);
            door2Unlocked = true;
            Debug.Log("문 2 잠금 해제");
        }

        // 문 3 잠금 해제: 문 2가 열렸을 때
        if (door2.open && !door3Unlocked)
        {
            UnlockDoor(door3);
            door3Unlocked = true;
            Debug.Log("문 3 잠금 해제");
        }
    }

    public void UnlockDoor(doorController door)
    {
        if (door.CloseControl)
        {
            door.CloseControl = false; // 잠금 해제
            Debug.Log($"{door.name} 잠금 해제");
        }
    }

    public void HandleInsectInteraction()
    {
        if (!door1Unlocked)
        {
            UnlockDoor(door1);
            door1Unlocked = true;
            Debug.Log("문 1 잠금 해제");
        }
    }
}
