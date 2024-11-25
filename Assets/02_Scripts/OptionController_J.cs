using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionController_J : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel; // 옵션 창
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button backButton;

    private bool isOptionOpen = false; // 옵션 창 열림 여부

    private void Awake()
    {
        // DontDestroyOnLoad로 옵션 UI 유지
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 옵션 창 초기화
        optionPanel.SetActive(false);

        // 슬라이더와 드롭다운 초기화
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
        resolutionDropdown.value = 0; // 기본 해상도

        // 이벤트 등록
        soundSlider.onValueChanged.AddListener(delegate { AdjustSound(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { AdjustSensitivity(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { AdjustResolution(); });
        backButton.onClick.AddListener(CloseOptionPanel);
    }

    private void Update()
    {
        // ESC 키 입력으로 옵션 창 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionPanel();
        }
    }

    private void ToggleOptionPanel()
    {
        isOptionOpen = !isOptionOpen;
        optionPanel.SetActive(isOptionOpen);

        // 옵션 창 열리면 게임 멈춤
        if (isOptionOpen)
        {
            Time.timeScale = 0; // 게임 일시정지
        }
        else
        {
            Time.timeScale = 1; // 게임 재개
        }
    }

    private void CloseOptionPanel()
    {
        isOptionOpen = false;
        optionPanel.SetActive(false);
        Time.timeScale = 1; // 게임 재개
    }

    void AdjustSound()
    {
        GameManager_J.Instance.SetMasterVolume(soundSlider.value / 100f);
        GameManager_J.Instance.UpdateAudioListener(); // AudioListener 업데이트
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
