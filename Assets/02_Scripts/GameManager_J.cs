using UnityEngine;

public class GameManager_J : MonoBehaviour
{
    public static GameManager_J Instance;

    public float masterVolume = 1f; // 전역 사운드 값 (0~1)
    public float mouseSensitivity = 5f; // 전역 마우스 감도 값 (1~10)

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
    }

    private void Start()
    {
        UpdateAudioListener();
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
