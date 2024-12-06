using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
//using UnityEngine.UIElements;

public class UIController_J : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel; // 옵션 패널
    [SerializeField] private GameObject UIPanel;
    [SerializeField] private Button startButton;     // 게임 시작 버튼
    [SerializeField] private Button optionButton;    // 옵션 열기 버튼
    [SerializeField] private Button quitButton;      // 게임 종료 버튼

    public String NextScene;

    void Start()
    {
        // 옵션 패널 비활성화
        optionPanel.SetActive(false);
        // 버튼 이벤트 등록
        startButton.onClick.AddListener(StartGame);
        optionButton.onClick.AddListener(ToggleOption);
        quitButton.onClick.AddListener(QuitGame);
    }

    // 게임 시작 버튼
    void StartGame()
    {
        UIPanel.SetActive(false);
        SceneManager.LoadScene(NextScene);
        GameManager_J.Instance.UpdateAudioListener();
    }

    // 옵션 버튼 (옵션 패널 열기/닫기)
    void ToggleOption()
    {
        optionPanel.SetActive(!optionPanel.activeSelf);
    }

    // 게임 종료 버튼
    void QuitGame()
    {
        Application.Quit();
    }
}
