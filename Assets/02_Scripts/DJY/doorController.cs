using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class doorController : MonoBehaviour
{
    public float doorOpenAngle = 90f; // 열릴 각도
    public float doorCloseAngle = 0f; // 닫힐 각도
    public float smooth = 2f; // 애니메이션 속도

    public roomControl connectedRoom1;
    public roomControl connectedRoom2;

    public GameObject manager;
    public bool CloseControl = false; // 인스펙터에서 체크해놓으면 문 잠기게

    [System.NonSerialized]
    public bool open = false; // 문 상태
    private bool isLocked = false;

    [System.NonSerialized]
    public bool EventOn = false;

    [Header("Room Settings")]
    public roomControl connectedRoom; // 연결된 방 컨트롤러

    void Update()
    {
        if (CloseControl == true) // true 면 문 닫히게
        {
            open = false;
        }
        // 문 상태에 따른 회전
        if (open == true)
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
        else
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
    }

    public void ChangeDoorState()
    {
        if (CloseControl == false)
        {
            open = !open; // 문 상태 변경
        }

        Debug.Log(open ? "문이 열렸습니다." : "문이 닫혔습니다.");

        if (isLocked)
        {
            Debug.Log("문이 잠겨 있습니다.");
            return;
        }


        // 연결된 방의 상태 변경
        if (connectedRoom != null)
        {
            connectedRoom.SetRoomObjectsState(open);
        }
    }
}
