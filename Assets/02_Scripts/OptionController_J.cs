using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionController_J : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel; // �ɼ� â
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button backButton;

    private bool isOptionOpen = false; // �ɼ� â ���� ����

    private void Awake()
    {
        // DontDestroyOnLoad�� �ɼ� UI ����
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // �ɼ� â �ʱ�ȭ
        optionPanel.SetActive(false);

        // �����̴��� ��Ӵٿ� �ʱ�ȭ
        soundSlider.minValue = 0;
        soundSlider.maxValue = 100;
        soundSlider.value = GameManager_J.Instance.masterVolume * 100;

        sensitivitySlider.minValue = 1;
        sensitivitySlider.maxValue = 10;
        sensitivitySlider.value = GameManager_J.Instance.mouseSensitivity;

        resolutionDropdown.options.Clear();
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1920x1080"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1280x780"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("720x480"));
        resolutionDropdown.value = 0; // �⺻ �ػ�

        // �̺�Ʈ ���
        soundSlider.onValueChanged.AddListener(delegate { AdjustSound(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { AdjustSensitivity(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { AdjustResolution(); });
        backButton.onClick.AddListener(CloseOptionPanel);
    }

    private void Update()
    {
        // ESC Ű �Է����� �ɼ� â ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionPanel();
        }
    }

    private void ToggleOptionPanel()
    {
        isOptionOpen = !isOptionOpen;
        optionPanel.SetActive(isOptionOpen);

        // �ɼ� â ������ ���� ����
        if (isOptionOpen)
        {
            Time.timeScale = 0; // ���� �Ͻ�����
        }
        else
        {
            Time.timeScale = 1; // ���� �簳
        }
    }

    private void CloseOptionPanel()
    {
        isOptionOpen = false;
        optionPanel.SetActive(false);
        Time.timeScale = 1; // ���� �簳
    }

    void AdjustSound()
    {
        GameManager_J.Instance.SetMasterVolume(soundSlider.value / 100f);
        GameManager_J.Instance.UpdateAudioListener(); // AudioListener ������Ʈ
        Debug.Log("AdjustSound: Master Volume = " + GameManager_J.Instance.masterVolume);
    }

    private void AdjustSensitivity()
    {
        GameManager_J.Instance.mouseSensitivity = sensitivitySlider.value;
    }

    private void AdjustResolution()
    {
        switch (resolutionDropdown.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
                break;
            case 1:
                Screen.SetResolution(1280, 780, FullScreenMode.Windowed);
                break;
            case 2:
                Screen.SetResolution(720, 480, FullScreenMode.Windowed);
                break;
        }
    }
}
