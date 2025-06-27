using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public Button selectSong;

    // Start is called before the first frame update
    void Start()
    {
        if (selectSong != null)
            selectSong.onClick.AddListener(() => GameManager.Instance.GoToGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
