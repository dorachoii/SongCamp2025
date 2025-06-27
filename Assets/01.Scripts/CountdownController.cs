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
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("순서1: CountdownController 생성 (Awake)");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            Debug.Log("순서2: CountdownController 이벤트 등록 (OnEnable)");
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            Debug.Log("순서3: CountdownController 이벤트 해제 (OnDisable)");
        }
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            Debug.Log("순서4: CountdownController 이벤트 등록 (Start)");
        }
    }

    void HandleGameStateChanged(GameState state)
    {
        Debug.Log($"순서5: CountdownController 상태 변경 감지 - {state}");

        if (state == GameState.Ready)
        {
            UIManager.Instance.ShowTutorialUI(true);
            StartCountdown();
        }
    }

    public void StartCountdown()
    {
        Debug.Log("순서6: 카운트다운 시작");
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        for (int i = 5; i > 0; i--)
        {
            countdownText.text = i.ToString();
            Debug.Log($"카운트다운: {i}");
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "START!";
        Debug.Log("카운트다운: START!");
        yield return new WaitForSeconds(1f);

        GameManager.Instance.SetGameState(GameState.Playing);
        Debug.Log("순서7: 게임 상태 → Playing 전환");
    }
}
