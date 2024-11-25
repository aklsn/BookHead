using UnityEngine;

public class GameManager_J : MonoBehaviour
{
    public static GameManager_J Instance;

    public float masterVolume = 1f; // ���� ���� �� (0~1)
    public float mouseSensitivity = 5f; // ���� ���콺 ���� �� (1~10)

    private AudioListener currentAudioListener;

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

    private void Start()
    {
        UpdateAudioListener();
    }
    /// <summary>
    /// ����� ������ �˾Ƽ� ������������.
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
