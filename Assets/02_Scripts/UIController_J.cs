using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;

public class UIController_J : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel; // �ɼ� �г�
    [SerializeField] private GameObject UIPanel;
    [SerializeField] private Button startButton;     // ���� ���� ��ư
    [SerializeField] private Button optionButton;    // �ɼ� ���� ��ư
    [SerializeField] private Button quitButton;      // ���� ���� ��ư

    void Start()
    {
        // �ɼ� �г� ��Ȱ��ȭ
        optionPanel.SetActive(false);
        // ��ư �̺�Ʈ ���
        startButton.onClick.AddListener(StartGame);
        optionButton.onClick.AddListener(ToggleOption);
        quitButton.onClick.AddListener(QuitGame);
    }

    // ���� ���� ��ư
    void StartGame()
    {
        UIPanel.SetActive(false);
        SceneManager.LoadScene("MapScene");
        GameManager_J.Instance.UpdateAudioListener();
    }

    // �ɼ� ��ư (�ɼ� �г� ����/�ݱ�)
    void ToggleOption()
    {
        optionPanel.SetActive(!optionPanel.activeSelf);
    }

    // ���� ���� ��ư
    void QuitGame()
    {
        Application.Quit();
    }
}
