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
    List<Transform> spawnRails;

    public List<NoteData> noteSpawnQueue = new List<NoteData>();
    public List<NoteData>[] noteSpawnQueue_perRail = new List<NoteData>[railCount];
    public List<NoteInstance>[] spawnedNotes_perRail = new List<NoteInstance>[railCount];

    CountdownController countdownController;
    bool isInitialized = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("순서0: NoteMaker 생성됨 (Awake)");
        }
        else
        {
            Destroy(gameObject);
        }

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
        Debug.Log("순서1: NoteMaker OnEnable");
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

    public void Restart()
    {
        Debug.Log("순서10: NoteMaker.Restart() 실행");
        currTime = 0f;
        ClearQueues();
        ReassignRails();
        // 노트 큐는 초기화하되, 실제 생성은 Playing 상태에서만
        InitializeNoteQueues();
        isInitialized = true;
        Debug.Log("순서11: NoteMaker 초기화 완료");
    }

    void Start()
    {
        Debug.Log("순서2: NoteMaker.Start()");
        ClearQueues();
        ReassignRails();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        countdownController = CountdownController.Instance;
        InitializeNoteQueues();
        isInitialized = true;
    }

    void HandleGameStateChanged(GameState state)
    {
        Debug.Log($"순서3: 상태 변경 감지됨 - {state}");

        if (state == GameState.Playing && !isInitialized)
        {
            Debug.Log("순서4: 게임 시작 - 초기화 실행");
            Restart();
        }
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing)
        {
            return;
        }

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
            if (spawnRails[railIndex] == null)
            {
                Debug.LogWarning($"spawnRails[{railIndex}] is null!");
                return;
            }

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

    void ReassignRails()
    {
        spawnRails = new List<Transform>();
        for (int i = 1; i <= 6; i++)
        {
            GameObject railObj = GameObject.Find($"noteFactory0{i}");

            if (railObj != null)
            {
                spawnRails.Add(railObj.transform);
            }
            else
            {
                Debug.LogWarning($"noteFactory0{i} not found!");
            }
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
        noteSpawnQueue.Clear();
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
        Debug.Log($"순서12: spawnQueue에 노트 {noteSpawnQueue.Count}개 등록됨");
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
        NoteData note1 = new NoteData();
        note1.railIdx = 0;
        note1.type = (int)NoteType.SHORT;
        note1.time = 1 * bpm;
        noteSpawnQueue.Add(note1);

        NoteData note2 = new NoteData();
        note2.railIdx = 1;
        note2.type = (int)NoteType.SHORT;
        note2.time = 1 * bpm;
        noteSpawnQueue.Add(note2);

        NoteData note3 = new NoteData();
        note3.railIdx = 2;
        note3.type = (int)NoteType.SHORT;
        note3.time = 3 * bpm;
        noteSpawnQueue.Add(note3);

        NoteData note4 = new NoteData();
        note4.railIdx = 3;
        note4.type = (int)NoteType.SHORT;
        note4.time = 4 * bpm;
        noteSpawnQueue.Add(note4);

        NoteData note5 = new NoteData();
        note5.railIdx = 4;
        note5.type = (int)NoteType.SHORT;
        note5.time = 5 * bpm;
        noteSpawnQueue.Add(note5);

        NoteData note6 = new NoteData();
        note6.railIdx = 5;
        note6.type = (int)NoteType.SHORT;
        note6.time = 6 * bpm;
        noteSpawnQueue.Add(note6);

        for (int i = 0; i < noteSpawnQueue.Count; i++)
        {
            noteSpawnQueue_perRail[noteSpawnQueue[i].railIdx].Add(noteSpawnQueue[i]);
        }
    }
}
