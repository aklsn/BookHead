using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityStandardAssets.Characters.FirstPerson;

public class UIController_J : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button backButton;

    private AudioSource audioSource;

    void Start()
    {
        // Initialize UI Components
        optionPanel.SetActive(false);
        audioSource = FindObjectOfType<AudioSource>();

        // Set initial values from GameManager
        soundSlider.minValue = 0;
        soundSlider.maxValue = 100;
        soundSlider.value = GameManager_J.Instance.masterVolume * 100;
        audioSource.volume = GameManager_J.Instance.masterVolume;      //재준 추가 사항

        sensitivitySlider.minValue = 1;
        sensitivitySlider.maxValue = 10;
        sensitivitySlider.value = GameManager_J.Instance.mouseSensitivity;

        // Populate resolution dropdown
        resolutionDropdown.options.Clear();
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1920x1080"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1280x780"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("720x480"));
        resolutionDropdown.value = GameManager_J.Instance.resolutionIndex;
        
        resolutionDropdown.RefreshShownValue();     //UI   업데이트

        // Add listeners to buttons and sliders
        startButton.onClick.AddListener(StartGame);
        optionButton.onClick.AddListener(ToggleOption);
        quitButton.onClick.AddListener(QuitGame);

        soundSlider.onValueChanged.AddListener(delegate { AdjustSound(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { AdjustSensitivity(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { AdjustResolution(); });
        backButton.onClick.AddListener(CloseOptionPanel);
    }

    void StartGame()
    {
        SceneManager.LoadScene("MapScene");
    }

    void ToggleOption()
    {
        optionPanel.SetActive(!optionPanel.activeSelf);
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void AdjustSound()
    {
        GameManager_J.Instance.masterVolume = soundSlider.value / 100f;
        audioSource.volume = GameManager_J.Instance.masterVolume;
        GameManager_J.Instance.SaveSettings();          //재준 : 파일저장
    }

    void AdjustSensitivity()
    {
        GameManager_J.Instance.mouseSensitivity = sensitivitySlider.value;
        Debug.Log("Sensitivity updated to : " + GameManager_J.Instance.mouseSensitivity);
        GameManager_J.Instance.SaveSettings();          //재준 : 파일저장
    }

    void AdjustResolution()
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
        
        GameManager_J.Instance.SaveSettings();          //재준 : 파일저장
    }

    void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }
}
