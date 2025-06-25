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

    private bool rightDragReady = false;
    private bool leftDragReady = false;

    void Start()
    {
        noteJudge = FindObjectOfType<NoteJudge>();
    }

    void Update()
    {
        HandleNoteInput();

        if (rightDragReady || leftDragReady)
        {
            HandleDragInput();
        }
    }

    private void HandleNoteInput()
    {
        for (int i = 0; i < railKeys.Length; i++)
        {
            if (Input.GetKeyDown(railKeys[i]))
            {
                rails[i].GetComponent<MeshRenderer>().material = activeMats[i];

                var noteList = NoteMaker.Instance.spawnedNotes_perRail[i];
                if (noteList.Count == 0) continue;

                var note = noteList[0];
                NoteType type = (NoteType)note.noteInfo.type;

                switch (type)
                {
                    case NoteType.SHORT:
                        noteJudge.JudgeReleasingTiming(i);
                        break;
                    case NoteType.DRAG_RIGHT:
                        if (noteJudge.JudgeTouchedTiming(i))
                        {
                            SetRightDragReady(true);
                        }

                        break;
                    case NoteType.DRAG_LEFT:

                        if (noteJudge.JudgeTouchedTiming(i))
                        {
                            SetLeftDragReady(true);
                        }
                        break;
                    case NoteType.LONG:
                        if (note.noteInfo.isLongNoteStart && noteJudge.JudgeTouchedTiming(i))
                        {
                            note.isHolding = true;
                        }
                        break;
                }
            }

            if (Input.GetKeyUp(railKeys[i]))
            {
                rails[i].GetComponent<MeshRenderer>().material = defaultMats[i];

                var noteList = NoteMaker.Instance.spawnedNotes_perRail[i];
                if (noteList.Count == 0) continue;

                var startNote = noteList[0];

                if (startNote.noteInfo.type == (int)NoteType.LONG)
                {
                    if (startNote != null && startNote.isHolding)
                    {
                        if (noteList.Count > 1 && noteList[1] != null)
                        {
                            noteJudge.JudgeReleasingTiming(i, 1);
                        }
                        else
                        {
                            noteJudge.JudgeReleasingTiming(i, 0);
                        }
                        startNote.isHolding = false;
                    }
                }

            }
        }
    }

    private void HandleDragInput()
    {
        if (rightDragReady)
        {
            foreach (var keyCode in new[] { KeyCode.J, KeyCode.K, KeyCode.L })
            {
                if (Input.GetKeyDown(keyCode))
                {
                    R_ringBuffer.Enqueue(new KeyInput(keyCode, Time.time));

                    if (R_ringBuffer.Count > 3) R_ringBuffer.Dequeue();

                    if (IsDragMatched(R_DragSequence, sequenceTimeLimit))
                    {
                        R_ringBuffer.Clear();
                        noteJudge.JudgeReleasingTiming(3);
                        SetRightDragReady(false);
                    }
                }
            }
        }

        if (leftDragReady)
        {
            foreach (var keyCode in new[] { KeyCode.F, KeyCode.D, KeyCode.S })
            {
                if (Input.GetKeyDown(keyCode))
                {
                    R_ringBuffer.Enqueue(new KeyInput(keyCode, Time.time));

                    if (R_ringBuffer.Count > 3) R_ringBuffer.Dequeue();

                    if (IsDragMatched(L_DragSequence, sequenceTimeLimit))
                    {
                        R_ringBuffer.Clear();
                        noteJudge.JudgeReleasingTiming(2);
                        SetLeftDragReady(false);
                    }
                }
            }
        }
    }

    private bool IsDragMatched(KeyCode[] expected, float timeLimit)
    {
        if (R_ringBuffer.Count < expected.Length) return false;

        var inputs = R_ringBuffer.ToArray();

        for (int i = 0; i < expected.Length; i++)
        {
            if (inputs[i].key != expected[i])
                return false;
        }

        float duration = inputs[inputs.Length - 1].time - inputs[0].time;
        return duration <= timeLimit;
    }




    private void SetRightDragReady(bool ready)
    {
        rightDragReady = ready;
        if (!ready) R_ringBuffer.Clear();
    }

    private void SetLeftDragReady(bool ready)
    {
        leftDragReady = ready;
        if (!ready) L_ringBuffer.Clear();
    }

}
