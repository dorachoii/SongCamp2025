using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private struct KeyInput
    {
        public KeyCode key;
        public float time;

        public KeyInput(KeyCode key, float time)
        {
            this.key = key;
            this.time = time;
        }
    }

    // ring Buffer로 구현
    private Queue<KeyInput> R_ringBuffer = new Queue<KeyInput>(3);
    private Queue<KeyInput> L_ringBuffer = new Queue<KeyInput>(3);
    private readonly KeyCode[] R_DragSequence = new[] { KeyCode.J, KeyCode.K, KeyCode.L };
    private readonly KeyCode[] L_DragSequence = new[] { KeyCode.F, KeyCode.D, KeyCode.S };
    private const float sequenceTimeLimit = 0.2f;

    void Start()
    {
        noteJudge = FindObjectOfType<NoteJudge>();
    }

    void Update()
    {
        HandleShortInput();
        HandleDragInput();
    }

    private void HandleShortInput()
    {
        for (int i = 0; i < railKeys.Length; i++)
        {
            if (Input.GetKeyDown(railKeys[i]))
            {
                rails[i].GetComponent<MeshRenderer>().material = activeMats[i];

                var noteList = NoteManager.Instance.spawnedNotes_perRail[i];
                if (noteList.Count == 0) continue;

                var note = noteList[0];
                NoteType type = (NoteType)note.noteInfo.type;

                if (type == NoteType.SHORT)
                {
                    noteJudge.JudgeShortNote(i);
                }
            }

            if (Input.GetKeyUp(railKeys[i]))
            {
                rails[i].GetComponent<MeshRenderer>().material = defaultMats[i];
            }
        }
    }

    private void HandleDragInput()
    {
        foreach (var keyCode in new[] { KeyCode.J, KeyCode.K, KeyCode.L })
        {
            if (Input.GetKeyDown(keyCode))
            {
                R_ringBuffer.Enqueue(new KeyInput(keyCode, Time.time));

                if (R_ringBuffer.Count > 3) R_ringBuffer.Dequeue();

                if (IsRingBufferMatched(R_DragSequence, sequenceTimeLimit))
                {
                    R_ringBuffer.Clear();
                }
            }
        }

        foreach (var keyCode in new[] { KeyCode.F, KeyCode.D, KeyCode.S })
        {
            if (Input.GetKeyDown(keyCode))
            {
                R_ringBuffer.Enqueue(new KeyInput(keyCode, Time.time));

                if (R_ringBuffer.Count > 3) R_ringBuffer.Dequeue();

                if (IsRingBufferMatched(L_DragSequence, sequenceTimeLimit))
                {
                    R_ringBuffer.Clear();
                    Debug.Log("success");
                }
            }
        }
    }

    private bool IsRingBufferMatched(KeyCode[] expected, float timeLimit)
    {
        if (R_ringBuffer.Count < expected.Length)return false;

        var inputs = R_ringBuffer.ToArray();

        for (int i = 0; i < expected.Length; i++)
        {
            if (inputs[i].key != expected[i])
                return false;
        }

        float duration = inputs[inputs.Length - 1].time - inputs[0].time;
        return duration <= timeLimit;
    }


}
