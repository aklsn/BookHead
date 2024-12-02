using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class OptionController_J : MonoBehaviour
{
    public static OptionController_J Instance; // 싱글톤 인스턴스

    [SerializeField] private GameObject optionPanel; // 옵션 창
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button backButton;

    private bool isOptionOpen = false; // 옵션 창 열림 여부

    private void Awake()
    {
        // 기존 OptionController_J 초기화 코드
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

        // EventSystem 관리
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
        // 옵션 창 초기화
        optionPanel.SetActive(false);

        // 슬라이더와 드롭다운 초기화
        soundSlider.minValue = 0;
        soundSlider.maxValue = 100;
        soundSlider.value = GameManager_J.Instance.masterVolume * 100;

        sensitivitySlider.minValue = 50;
        sensitivitySlider.maxValue = 1000;
        sensitivitySlider.value = GameManager_J.Instance.mouseSensitivity*100;

        resolutionDropdown.options.Clear();
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1920x1080"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1280x780"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("720x480"));

        resolutionDropdown.value = GameManager_J.Instance.resolutionIndex;
        resolutionDropdown.RefreshShownValue();

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

        // UI 클릭만 허용
        if (isOptionOpen && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI Clicked! Input on game objects is blocked.");
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
            Cursor.lockState = CursorLockMode.None; // 커서 잠금 해제
            Cursor.visible = true; // 커서 표시
        }
        else
        {
            Time.timeScale = 1; // 게임 재개
            Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
            Cursor.visible = false; // 커서 숨기기
        }
    }

    public void CloseOptionPanel()
    {
        isOptionOpen = false;
        optionPanel.SetActive(false);
        Time.timeScale = 1; // 게임 재개
    }

    public bool IsOptionOpen()
    {
        return isOptionOpen; // 옵션 창 상태 반환
    }

    private void AdjustSound()
    {
        GameManager_J.Instance.SetMasterVolume(soundSlider.value / 100f);
        GameManager_J.Instance.UpdateAudioListener(); // AudioListener 업데이트
        GameManager_J.Instance.SavaOptionData();
    }

    private void AdjustSensitivity()
    {
        GameManager_J.Instance.mouseSensitivity = sensitivitySlider.value;
        GameManager_J.Instance.SavaOptionData();

        // PlayerControl의 mouseSensitivity 업데이트
        var player = FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            player.mouseSensitivity = sensitivitySlider.value;
            Debug.Log("PlayerControl sensitivity updated: " + player.mouseSensitivity);
        }
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

        GameManager_J.Instance.SavaOptionData();
    }
}
