using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OptionController_J : MonoBehaviour
{
    public static OptionController_J Instance; // 싱글톤 인스턴스

    [SerializeField] private GameObject optionPanel; // 옵션 창
    [SerializeField] private GameObject Back;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button backButton;

    private bool isOptionOpen = false; // 옵션 창 열림 여부
    private string menuSceneName = "MenuScene"; // Back 패널이 활성화될 씬 이름

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

        EnsureEventSystem();
    }

    private void EnsureEventSystem()
    {
        if (EventSystem.current == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            DontDestroyOnLoad(eventSystem);
        }
        else
        {
            DontDestroyOnLoad(EventSystem.current.gameObject);
        }
    }

    private void Start()
    {
        optionPanel.SetActive(false);
        Back.SetActive(IsInMenuScene());

        soundSlider.minValue = 0;
        soundSlider.maxValue = 100;
        soundSlider.value = GameManager_J.Instance.masterVolume * 100;

        sensitivitySlider.maxValue = 100;
        sensitivitySlider.minValue = 10;
        sensitivitySlider.value = GameManager_J.Instance.mouseSensitivity * 100;

        resolutionDropdown.options.Clear();
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("2560x1600"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1920x1080"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1280x780"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("720x480"));

        resolutionDropdown.value = GameManager_J.Instance.resolutionIndex;
        resolutionDropdown.RefreshShownValue();

        soundSlider.onValueChanged.AddListener(delegate { AdjustSound(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { AdjustSensitivity(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { AdjustResolution(); });
        backButton.onClick.AddListener(CloseOptionPanel);
    }

    private void Update()
    {
        // ESC 키 입력 무시 조건
        if (IsInMenuScene())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionPanel();
        }
    }

    public void ToggleOptionPanel()
    {
        isOptionOpen = !isOptionOpen;
        optionPanel.SetActive(isOptionOpen);
        Back.SetActive(isOptionOpen ? false : IsInMenuScene());

        if (isOptionOpen)
        {
            Time.timeScale = 0; // 게임 일시정지
            Cursor.lockState = CursorLockMode.None; // 커서 잠금 해제
            Cursor.visible = true; // 커서 표시
        }
        else
        {
            Time.timeScale = 1; // 게임 재개
            Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
            Cursor.visible = false; // 커서 숨기기
            ResetEventSystemFocus();
        }
    }

    public void CloseOptionPanel()
    {
        isOptionOpen = false;
        optionPanel.SetActive(false);
        Back.SetActive(IsInMenuScene());
        Time.timeScale = 1; // 게임 재개

        if (!IsInMenuScene())
        {
            Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
            Cursor.visible = false; // 커서 숨기기
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; // MenuScene에서는 커서 잠금 해제
            Cursor.visible = true; // MenuScene에서는 커서 표시
        }

        ResetEventSystemFocus();
    }

    public bool IsOptionOpen()
    {
        return isOptionOpen;
    }

    private void AdjustSound()
    {
        GameManager_J.Instance.SetMasterVolume(soundSlider.value / 100f);
        GameManager_J.Instance.UpdateAudioListener();
        GameManager_J.Instance.SavaOptionData();
    }

    private void AdjustSensitivity()
    {
        GameManager_J.Instance.mouseSensitivity = sensitivitySlider.value;
        GameManager_J.Instance.SavaOptionData();

        var player = FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            player.mouseSensitivity = sensitivitySlider.value;
        }
    }

    private void AdjustResolution()
    {
        switch (resolutionDropdown.value)
        {
            case 0:
                Screen.SetResolution(2560, 1600, FullScreenMode.FullScreenWindow);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                
                break;
            case 2:
                Screen.SetResolution(1280, 780, FullScreenMode.FullScreenWindow);
                break;
            case 3:
                Screen.SetResolution(720, 480, FullScreenMode.FullScreenWindow);
                break;
        }

        GameManager_J.Instance.SavaOptionData();
    }

    private void ResetEventSystemFocus()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private bool IsInMenuScene()
    {
        return SceneManager.GetActiveScene().name == menuSceneName;
    }
}
