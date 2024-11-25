using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_J : MonoBehaviour
{
    public static GameManager_J Instance;

    public float masterVolume = 1f; // 전역 사운드 값 (0~1)
    public float mouseSensitivity = 5f; // 전역 마우스 감도 값 (1~10)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }
}
