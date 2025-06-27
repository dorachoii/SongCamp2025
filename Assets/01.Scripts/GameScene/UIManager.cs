using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
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
        tutorialCanvas.gameObject.SetActive(isOn);
        scoreCanvas.gameObject.SetActive(!isOn);
    }


}
