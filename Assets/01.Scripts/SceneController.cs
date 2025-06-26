using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {

        }
    }


    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private bool isPaused = false;

    public void PauseGame()
    {
        if (isPaused) return;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void TogglePause()
    {
        if (isPaused) ResumeGame();
        else PauseGame();
    }

    public void QuitToLobby()
    {
        Time.timeScale = 1f;
        isPaused = false;
        LoadSceneByIndex(0);
    }
}
