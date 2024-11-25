using System.IO;
using UnityEngine;

public class GameManager_J : MonoBehaviour
{
    public static GameManager_J Instance;

    public float masterVolume = 1f; // 마스터 볼륨 (0~1)
    public float mouseSensitivity = 5f; // 마우스 감도 (1~10)
    public int resolutionIndex = 0; // 해상도 인덱스 (0: 1920x1080, 1: 1280x720, 2: 720x480)

    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
         
        filePath = Application.persistentDataPath + "/GameSetting.json";
        LoadSettings(); // 게임 실행 시 설정 불러오기
    }

    
    /// <summary>
    /// 재준 파일 저장 불러오기 스크립트 추가
    /// </summary>
    public void SaveSettings()
    {
        GameSetting settings = new GameSetting
        {
            masterVolume = masterVolume,
            mouseSensitivity = mouseSensitivity,
            resolutionIndex = resolutionIndex
        };

        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(filePath, json);
        Debug.Log("해당 위치에 저장됨: " + filePath);
    }

    public void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameSetting settings = JsonUtility.FromJson<GameSetting>(json);

            masterVolume = settings.masterVolume;
            mouseSensitivity = settings.mouseSensitivity;
            resolutionIndex = settings.resolutionIndex;

            Debug.Log("해당 위치에 저장파일 불러옴: " + filePath);
        }
        else
        {
            Debug.Log("기본 세팅 파일 찾을 수 없음");
        }
    }
}