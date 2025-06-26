using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject resultPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalComboText;
    public TextMeshProUGUI finalGradeText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
}
