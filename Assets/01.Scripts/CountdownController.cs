using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownController : MonoBehaviour
{
    public static CountdownController Instance { get; private set; }
    public TMP_Text countdownText;

    void Awake()
    {
       Instance = this;
    }

    void OnDestroy()
    {
        if(Instance == this)
        Instance = null;
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Ready)
        {
            UIManager.Instance.ShowTutorialUI(true);
            StartCountdown();
        }
    }

    public void StartCountdown()
    {
        if (this == null) return; // 혹시 Destroy된 경우 방지
        print("순서5 : 카운트다운 시작");
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        for (int i = 5; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "START!";
        yield return new WaitForSeconds(1f);

        GameManager.Instance.SetGameState(GameState.Playing);
    }
}
