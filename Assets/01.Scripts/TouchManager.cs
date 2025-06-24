using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public GameObject[] rails;
    public Material[] activeMats;
    public Material[] defaultMats;
    private NoteJudge noteJudge;

    // 레일에 대응되는 키보드 키들
    private KeyCode[] railKeys = new KeyCode[6]
    {
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L
    };

    void Start()
    {
        noteJudge = FindObjectOfType<NoteJudge>();
    }

    void Update()
    {
        for (int i = 0; i < railKeys.Length; i++)
        {
            if (Input.GetKeyDown(railKeys[i]))
            {
                // 눌렸을 때 → 색 변경 + 판정
                rails[i].GetComponent<MeshRenderer>().material = activeMats[i];
                noteJudge.JudgeShortNote(i);
            }

            if (Input.GetKeyUp(railKeys[i]))
            {
                // 손 뗐을 때 → 색 복구
                rails[i].GetComponent<MeshRenderer>().material = defaultMats[i];
            }
        }
    }
}
