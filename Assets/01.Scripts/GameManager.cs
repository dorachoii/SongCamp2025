using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    private bool isGameOver = false;

    void OnEnable()
    {
        //NoteMaker.OnAllNotesSpawned += EndGame;
    }

    void OnDisable()
    {
        //NoteMaker.OnAllNotesSpawned -= EndGame;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!isGameOver && CheckGameEnd())
        {
            //EndGame();
        }
    }

    private bool CheckGameEnd()
    {
        // 예: 모든 노트가 사라졌는지 확인
        foreach (var list in NoteMaker.Instance.spawnedNotes_perRail)
        {
            if (list.Count > 0) return false;
        }
        return true;
    }

    private void EndGame()
    {
        isGameOver = true;
        UIManager.Instance.ShowResultUI();
    }
}
