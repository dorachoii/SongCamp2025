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
                // 눌렸을 때 → 색 변경
                rails[i].GetComponent<MeshRenderer>().material = activeMats[i];

                var noteList = NoteManager.Instance.spawnedNotes_perRail[i];
                if (noteList.Count == 0) continue;

                var note = noteList[0];
                NoteType type = (NoteType)note.noteInfo.type;

                if (type == NoteType.SHORT)
                {
                    noteJudge.JudgeShortNote(i);
                }
                else if (type == NoteType.DRAG_RIGHT)
                {
                    noteJudge.JudgeDragNote(true);
                }
                else if (type == NoteType.DRAG_LEFT)
                {
                    noteJudge.JudgeDragNote(false);
                }
            }

            if (Input.GetKeyUp(railKeys[i]))
            {
                // 손 뗐을 때 → 색 복구
                rails[i].GetComponent<MeshRenderer>().material = defaultMats[i];
            }
        }
    }
}
