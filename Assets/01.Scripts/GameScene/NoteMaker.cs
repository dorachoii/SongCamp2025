using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using Unity.VisualScripting;
using UnityEngine;

// 생성과 파괴 담당
public class NoteMaker : MonoBehaviour
{
    public static NoteMaker Instance { get; private set; }

    const int railCount = 6;
    float currTime;
    int bpm = 70;

    public GameObject[] notePrefabs;
    public List<Transform> spawnRails;

    public List<NoteData> noteSpawnQueue = new List<NoteData>();
    public List<NoteData>[] noteSpawnQueue_perRail = new List<NoteData>[railCount];
    public List<NoteInstance>[] spawnedNotes_perRail = new List<NoteInstance>[railCount];

    CountdownController countdownController;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        ClearQueues();

        // longNote위해서
        NoteInstance.GetNextNoteInRail = (railIdx, currentNote) =>
        {
            var list = spawnedNotes_perRail[railIdx];
            int idx = list.IndexOf(currentNote);
            if (idx >= 0 && idx + 1 < list.Count)
                return list[idx + 1];
            return null;
        };
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    void Start()
    {
        countdownController = CountdownController.Instance;
        InitializeNoteQueues();
    }



    void HandleGameStateChanged(GameState state)
    {
        currTime = 0f;

        ClearQueues();
        InitializeNoteQueues();
    }


    void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        currTime += Time.deltaTime;

        for (int i = 0; i < noteSpawnQueue_perRail.Length; i++)
        {
            SpawnNoteAtRail(i);
        }
    }

    void SpawnNoteAtRail(int railIndex)
    {
        var queue = noteSpawnQueue_perRail[railIndex];

        if (queue.Count == 0) return;

        float spawnTime = queue[0].time / bpm;

        if (currTime >= spawnTime)
        {
            GameObject prefab = notePrefabs[queue[0].type];
            GameObject note = Instantiate(
                prefab,
                spawnRails[railIndex].position + Vector3.forward * (-0.5f),
                prefab.transform.rotation
            );

            note.transform.SetParent(spawnRails[railIndex]);

            NoteInstance noteInstance = note.GetComponent<NoteInstance>();
            MakeNote(railIndex, noteInstance);
        }
    }

    void MakeNote(int n, NoteInstance note)
    {
        note.noteInfo = noteSpawnQueue_perRail[n][0];
        spawnedNotes_perRail[n].Add(note);
        noteSpawnQueue_perRail[n].RemoveAt(0);
    }

    public void RemoveNote(NoteInstance note)
    {
        int rail = note.noteInfo.railIdx;
        if (spawnedNotes_perRail[rail].Contains(note))
        {
            spawnedNotes_perRail[rail].Remove(note);
        }
    }

    void ClearQueues()
    {
        for (int i = 0; i < spawnedNotes_perRail.Length; i++)
        {
            spawnedNotes_perRail[i] = new List<NoteInstance>();
        }
        for (int i = 0; i < noteSpawnQueue_perRail.Length; i++)
        {
            noteSpawnQueue_perRail[i] = new List<NoteData>();
        }
    }

    void InitializeNoteQueues()
    {
        ClearQueues();
        TestSHORT();
        //TestDRAG();
        //TestLONG();
        //TestMIX();
        //SampleSong.Instance.InputTestFLOP();
    }


    void TestLONG()
    {
        NoteData note = new NoteData();
        note.railIdx = 3;
        note.type = (int)NoteType.LONG;
        note.time = 3 * bpm;
        note.isLongNoteStart = true;
        noteSpawnQueue.Add(note);

        note.railIdx = 3;
        note.type = (int)NoteType.LONG;
        note.time = 5 * bpm;
        note.isLongNoteStart = false;
        noteSpawnQueue.Add(note);

        for (int i = 0; i < noteSpawnQueue.Count; i++)
        {
            noteSpawnQueue_perRail[noteSpawnQueue[i].railIdx].Add(noteSpawnQueue[i]);
        }
    }

    void TestDRAG()
    {
        NoteData note = new NoteData();
        note.railIdx = 2;
        note.type = (int)NoteType.DRAG_LEFT;
        note.time = 1 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 3;
        note.type = (int)NoteType.DRAG_RIGHT;
        note.time = 3 * bpm;
        noteSpawnQueue.Add(note);

        for (int i = 0; i < noteSpawnQueue.Count; i++)
        {
            noteSpawnQueue_perRail[noteSpawnQueue[i].railIdx].Add(noteSpawnQueue[i]);
        }
    }


    void TestSHORT()
    {
        NoteData note = new NoteData();

        note.railIdx = 0;
        note.type = (int)NoteType.SHORT;
        note.time = 1 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 1;
        note.type = (int)NoteType.SHORT;
        note.time = 1 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 2;
        note.type = (int)NoteType.SHORT;
        note.time = 3 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 3;
        note.type = (int)NoteType.SHORT;
        note.time = 4 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 4;
        note.type = (int)NoteType.SHORT;
        note.time = 5 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 5;
        note.type = (int)NoteType.SHORT;
        note.time = 6 * bpm;
        noteSpawnQueue.Add(note);

        for (int i = 0; i < noteSpawnQueue.Count; i++)
        {
            noteSpawnQueue_perRail[noteSpawnQueue[i].railIdx].Add(noteSpawnQueue[i]);
        }
    }

    void TestMIX()
    {
        NoteData note = new NoteData();
        note.railIdx = 3;
        note.type = (int)NoteType.LONG;
        note.time = 1 * bpm;
        note.isLongNoteStart = true;
        noteSpawnQueue.Add(note);

        note.railIdx = 3;
        note.type = (int)NoteType.LONG;
        note.time = 3 * bpm;
        note.isLongNoteStart = false;
        noteSpawnQueue.Add(note);

        note.railIdx = 0;
        note.type = (int)NoteType.SHORT;
        note.time = 6 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 1;
        note.type = (int)NoteType.SHORT;
        note.time = 7 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 2;
        note.type = (int)NoteType.SHORT;
        note.time = 8 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 3;
        note.type = (int)NoteType.SHORT;
        note.time = 9 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 4;
        note.type = (int)NoteType.SHORT;
        note.time = 10 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 5;
        note.type = (int)NoteType.SHORT;
        note.time = 11 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 2;
        note.type = (int)NoteType.DRAG_LEFT;
        note.time = 15 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 3;
        note.type = (int)NoteType.DRAG_RIGHT;
        note.time = 17 * bpm;
        noteSpawnQueue.Add(note);

        for (int i = 0; i < noteSpawnQueue.Count; i++)
        {
            noteSpawnQueue_perRail[noteSpawnQueue[i].railIdx].Add(noteSpawnQueue[i]);
        }
    }
}
