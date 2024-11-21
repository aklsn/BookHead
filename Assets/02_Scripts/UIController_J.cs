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

    // New UI elements for settings
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button backButton;

    [SerializeField] private MouseLook mouseLook;

    private AudioSource audioSource;
    private Camera mainCamera;

    void Start()
    {
        // Initialize UI Components
        optionPanel.SetActive(false);
        audioSource = FindObjectOfType<AudioSource>();
        mainCamera = Camera.main;

        // Set initial values for sliders
        soundSlider.minValue = 0;
        soundSlider.maxValue = 100;
        soundSlider.value = audioSource.volume * 100;

        sensitivitySlider.minValue = 1;
        sensitivitySlider.maxValue = 10;
        sensitivitySlider.value = mouseLook.XSensitivity; // Default sensitivity

        // Populate resolution dropdown
        resolutionDropdown.options.Clear();
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1920x1080"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1280x780"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("720x480"));
        resolutionDropdown.value = 0; // Default resolution index

        // Add listeners to main menu buttons
        startButton.onClick.AddListener(StartGame);
        optionButton.onClick.AddListener(ToggleOption);
        quitButton.onClick.AddListener(QuitGame);

        // Add listeners to option panel controls
        soundSlider.onValueChanged.AddListener(delegate { AdjustSound(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { AdjustSensitivity(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { AdjustResolution(); }); // 주석 해제
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
        audioSource.volume = soundSlider.value / 100f;
    }

    void AdjustSensitivity()
    {
        mouseLook.XSensitivity = sensitivitySlider.value;
        mouseLook.YSensitivity = sensitivitySlider.value;
        Debug.Log("Sensitivity updated to : " + sensitivitySlider.value);
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
    }

    void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }
}
