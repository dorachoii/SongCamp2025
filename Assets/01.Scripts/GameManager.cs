using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ready,
    Playing,
    Paused,
    End
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("순서0: GameManager 생성 (Awake)");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetGameState(GameState newState)
    {
        Debug.Log($"순서1: SetGameState 호출 → {newState}");
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Ready:
            case GameState.Playing:
                Time.timeScale = 1;
                Debug.Log("순서2: Time.timeScale = 1");
                break;
            case GameState.Paused:
            case GameState.End:
                Time.timeScale = 0;
                Debug.Log("순서2: Time.timeScale = 0");
                break;
        }

        OnGameStateChanged?.Invoke(newState);
        Debug.Log("순서3: OnGameStateChanged 이벤트 호출");
    }

    public void Pause()
    {
        Debug.Log("순서4: 게임 일시정지 (Pause)");
        SongManager.Instance.PauseSong();
        SetGameState(GameState.Paused);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Debug.Log("순서5: 게임 재개 (Resume)");
        SongManager.Instance.ResumeSong();
        Time.timeScale = 1;
        SetGameState(GameState.Playing);
    }

    public void Retry()
    {
        Debug.Log("순서6: Retry 호출 - 씬 리로드");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToLobby()
    {
        Debug.Log("순서8: 로비로 이동");
        Time.timeScale = 1;
        SceneManager.LoadScene("LobbyScene");
    }
}
