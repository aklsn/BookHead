using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_J : MonoBehaviour
{
    [Header("Door Settings")]
    public doorController door1; // ù ��° ��
    public doorController door2; // �� ��° ��
    public doorController door3; // �� ��° ��

    private bool door1Unlocked = false; // �� 1 ��� ���� ����
    private bool door2Unlocked = false; // �� 2 ��� ���� ����
    private bool door3Unlocked = false; // �� 3 ��� ���� ����

    private void Update()
    {
        // �� 2 ��� ����: �� 1�� ������ ��
        if (door1.open && !door2Unlocked)
        {
            UnlockDoor(door2);
            door2Unlocked = true;
            Debug.Log("�� 2 ��� ����");
        }

        // �� 3 ��� ����: �� 2�� ������ ��
        if (door2.open && !door3Unlocked)
        {
            UnlockDoor(door3);
            door3Unlocked = true;
            Debug.Log("�� 3 ��� ����");
        }
    }

    public void UnlockDoor(doorController door)
    {
        if (door.CloseControl)
        {
            door.CloseControl = false; // ��� ����
            Debug.Log($"{door.name} ��� ����");
        }
    }

    public void HandleInsectInteraction()
    {
        if (!door1Unlocked)
        {
            UnlockDoor(door1);
            door1Unlocked = true;
            Debug.Log("�� 1 ��� ����");
        }
    }
}
