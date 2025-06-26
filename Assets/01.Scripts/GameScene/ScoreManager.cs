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
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI comboText; // ✅ 추가

    public Slider scoreSlider;

    private float score;
    private int combo;
    private int maxScore;

    private Coroutine comboDisplayCoroutine; // ✅ 추가

    private readonly Dictionary<JudgeResult, int> baseJudgeScores = new Dictionary<JudgeResult, int>
    {
        { JudgeResult.Miss , 0},
        { JudgeResult.Bad, 100 },
        { JudgeResult.Good, 300 },
        { JudgeResult.Great, 600 },
        { JudgeResult.Excellent, 1000 }
    };

    private readonly Dictionary<NoteType, float> noteTypeMultiplier = new Dictionary<NoteType, float>
    {
        { NoteType.SHORT, 1.0f },
        { NoteType.LONG, 1.5f },
        { NoteType.DRAG_RIGHT, 2.0f },
        { NoteType.DRAG_LEFT, 2.0f }
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
        score = 0;
        combo = 0;
        maxScore = 70000;
        scoreSlider.maxValue = maxScore;
        scoreSlider.value = 0;
        gradeText.text = "";
        numScore.text = "Score : 0";
        if (comboText != null) comboText.text = "";
    }

    private void HandleNoteJudged(NoteJudgedEventData data)
    {
        if (!baseJudgeScores.TryGetValue(data.result, out int baseScore) ||
            !noteTypeMultiplier.TryGetValue(data.noteType, out float multiplier))
            return;

        int finalScore = Mathf.RoundToInt(baseScore * multiplier);
        score += finalScore;
        scoreSlider.value = score;
        numScore.text = "Score : " + score;

        if (data.result == JudgeResult.Miss)
        {
            combo = 0;
            if (comboDisplayCoroutine != null)
            {
                
                StopCoroutine(comboDisplayCoroutine);
                comboText.text = "";
            }
        }
        else
        {
            combo++;

            if (combo % 5 == 0)
            {
                if (comboDisplayCoroutine != null)
                    StopCoroutine(comboDisplayCoroutine);

                comboDisplayCoroutine = StartCoroutine(ShowComboText(combo));
            }
        }

        StartShowScoreText(data.result.ToString(), data.railIndex, finalScore);
        UpdateGrade();
    }

    private void UpdateGrade()
    {
        float ratio = score / maxScore;
        string grade = "F";

        if (ratio >= 0.9f) grade = "S";
        else if (ratio >= 0.8f) grade = "A";
        else if (ratio >= 0.7f) grade = "B";
        else if (ratio >= 0.6f) grade = "C";
        else if (ratio >= 0.5f) grade = "D";

        gradeText.text = "Grade : " + grade;
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

    private IEnumerator ShowComboText(int combo)
    {
        comboText.text = combo + " Combo!";
        yield return new WaitForSeconds(1.0f);
        comboText.text = "";
    }

    public float GetScoreRatio() => score / maxScore;
    public int GetCombo() => combo;
    public string GetGrade() => gradeText.text;
}
