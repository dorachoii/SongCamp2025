using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    public CountdownController countdownController;

    public GameObject scoreCanvas;
    public GameObject tutorialCanvas;

    public GameObject resultPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalComboText;
    public TextMeshProUGUI finalGradeText;

    public GameObject pausePanel;

    // === 버튼 필드 추가 ===
    public Button retryButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button backToLobbyButton;
    public Button cancelButton;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ShowTutorialUI(true);
        
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;

        // === 버튼 이벤트 할당 ===
        if (retryButton != null)
            retryButton.onClick.AddListener(() => GameManager.Instance.Retry());
        if (pauseButton != null)
            pauseButton.onClick.AddListener(() => GameManager.Instance.Pause());
        if (resumeButton != null)
            resumeButton.onClick.AddListener(() => GameManager.Instance.Resume());
        if (backToLobbyButton != null)
            backToLobbyButton.onClick.AddListener(() => GameManager.Instance.GoToLobby());
        if (cancelButton != null)
            cancelButton.onClick.AddListener(() => GameManager.Instance.Resume());
    }

    void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Ready)
        {
            ShowTutorialUI(true);
        }

        if (state == GameState.Playing)
        {
            ShowTutorialUI(false);
            ShowPauseUI(false);
        }


        if (state == GameState.End)
            StartCoroutine(ShowResultAfterDelay(2f));

        if (state == GameState.Paused)
        {
            ShowPauseUI(true);
        }
        
    }

    public void ShowPauseUI(bool isOn)
    {
        if (pausePanel == null) return;
        pausePanel.SetActive(isOn);
    }

    IEnumerator ShowResultAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    ShowResultUI();
}

    public void ShowResultUI()
    {
        resultPanel.SetActive(true);

        float scoreRatio = ScoreManager.instance.GetScoreRatio();
        int maxCombo = ScoreManager.instance.GetCombo();
        string grade = ScoreManager.instance.GetGrade();

        finalScoreText.text = $"Score Rate: {(scoreRatio * 100f):F1}%";
        finalComboText.text = $"Max Combo: {maxCombo}";
        finalGradeText.text = $"Grade: {grade}";
    }

    public void ShowTutorialUI(bool isOn)
    {
        if (tutorialCanvas == null || scoreCanvas == null) return;
        tutorialCanvas.gameObject.SetActive(isOn);
        scoreCanvas.gameObject.SetActive(!isOn);
    }


}
