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
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetGameState(GameState newState)
    {
        print($"현재 게임 상태: {newState}");
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Ready:
            case GameState.Playing:
                Time.timeScale = 1; 
                break;
            case GameState.Paused:
            case GameState.End:
                Time.timeScale = 0;
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }


    public void Pause()
    {
        SongManager.Instance.PauseSong();
        SetGameState(GameState.Paused);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        SongManager.Instance.ResumeSong();
        Time.timeScale = 1;
        SetGameState(GameState.Playing);

    }

    public void Retry()
    {
        Time.timeScale = 1;
        SetGameState(GameState.Ready);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToLobby()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LobbyScene");
    }
}