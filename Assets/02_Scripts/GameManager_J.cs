using System.IO;
using UnityEngine;

public class GameManager_J : MonoBehaviour
{
    public static GameManager_J Instance;

    public float masterVolume = 1f; // 전역 사운드 값 (0~1)
    public float mouseSensitivity = 5f; // 전역 마우스 감도 값 (1~10)
    public int resolutionIndex = 0;     //재준 : 해상도 인덱스 (0 : 1920x1080 , 1 : 1280x780 , 2 : 720x480)

    private string _filePath;   //재준 : 파일경로
    private AudioListener currentAudioListener;

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
        
        _filePath = Application.persistentDataPath +  "/OptionData.json";
        LoadOptionData();
    }

    private void Start()
    {
        UpdateAudioListener();
    }

    public void SavaOptionData()
    {
        OptionData data = new OptionData
        {
            masterVolume = masterVolume,
            mouseSensitivity = mouseSensitivity,
            resolutionIndex = resolutionIndex
        };
        
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_filePath, json);
        Debug.Log("옵션값 저장 됨 : " + _filePath);
    }

    public void LoadOptionData()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            OptionData data = JsonUtility.FromJson<OptionData>(json);
            
            masterVolume = data.masterVolume;
            mouseSensitivity = data.mouseSensitivity;
            resolutionIndex = data.resolutionIndex;
            
            Debug.Log("불러옴 : " + _filePath);
        }
        else
        {
            Debug.Log("파일을 찾을 수 없음");
        }
    }
    
    /// <summary>
    /// 오디오 리스너 알아서 가져오게해줌.
    /// </summary>
    public void UpdateAudioListener()
    {
        currentAudioListener = FindObjectOfType<AudioListener>();

        if (currentAudioListener != null)
        {
            AudioListener.volume = masterVolume;
            Debug.Log("AudioListener updated: Volume set to " + masterVolume);
        }
        else
        {
            Debug.LogWarning("No active AudioListener found!");
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;

        if (currentAudioListener != null)
        {
            AudioListener.volume = masterVolume;
            Debug.Log("Master volume set to: " + masterVolume);
        }
    }
}
