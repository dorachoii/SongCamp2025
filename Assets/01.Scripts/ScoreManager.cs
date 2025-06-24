using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI textScore;
    public TextMeshProUGUI numScore;
    public TextMeshProUGUI[] score4rails;

    public Slider scoreSlider;
    public int maxScore = 10000;

    float score;

    private Dictionary<JudgeResult, int> judgeScores = new Dictionary<JudgeResult, int>
    {
        { JudgeResult.Bad, 100 },
        { JudgeResult.Good, 300 },
        { JudgeResult.Great, 600 },
        { JudgeResult.Excellent, 1000 }
    };

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        NoteJudge.OnNoteJudged += HandleNoteJudged;
    }

    private void OnDisable()
    {
        NoteJudge.OnNoteJudged -= HandleNoteJudged;
    }

    private void Start()
    {
        SCORE = 0;
        scoreSlider.maxValue = maxScore;
    }

    public float SCORE
    {
        get => score;
        set
        {
            score = value;
            numScore.text = "Score : " + score;
            scoreSlider.value = score;
        }
    }

    public void SetSCORE(int value)
    {
        score = value;
    }

    public float GetSCORE()
    {
        return score;
    }

    private void HandleNoteJudged(JudgeResult result, int railIdx)
    {
        if (!judgeScores.TryGetValue(result, out int addScore)) return;

        SCORE += addScore;
        StartShowScoreText(result.ToString(), railIdx, addScore);
    }

    public IEnumerator showScoreText(string label, int railIdx, int score)
    {
        textScore.text = label + "!";

        if (label != "Miss")
        {
            score4rails[railIdx].text = "+" + score;
        }

        yield return new WaitForSeconds(0.8f);

        score4rails[railIdx].text = "";
        textScore.text = "";
    }

    public void StartShowScoreText(string label, int railIdx, int score)
    {
        StartCoroutine(showScoreText(label, railIdx, score));
    }
}
