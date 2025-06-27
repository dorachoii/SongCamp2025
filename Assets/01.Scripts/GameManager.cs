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
            DontDestroyOnLoad(gameObject);
            print("순서1 : GameManager 생성");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Bootstrap")
        {
            GoToLobby();
        }
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        print($"순서2 : SetGameState 호출 : {newState}");

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
        UIManager.Instance.ShowPauseUI(true);
        SongManager.Instance.PauseSong();
        SetGameState(GameState.Paused);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        UIManager.Instance.ShowPauseUI(false);
        SongManager.Instance.ResumeSong();
        Time.timeScale = 1;
        SetGameState(GameState.Playing);
    }

    public void Retry()
    {
        Time.timeScale = 1;
        //SetGameState(GameState.Ready);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void GoToLobby()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LobbyScene");
    }

    public void GoToGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("PlayScene");
    }

}
