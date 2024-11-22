using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_J : MonoBehaviour
{
    public static GameManager_J Instance;

    public float masterVolume = 1f; // ���� ���� �� (0~1)
    public float mouseSensitivity = 5f; // ���� ���콺 ���� �� (1~10)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
        }
    }
}
