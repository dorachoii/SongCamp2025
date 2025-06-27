using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneInitilaizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SetGameState(GameState.Ready);
    }
}
