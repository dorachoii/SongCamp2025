using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    const int railCount = 6;
    float currTime;
    int bpm = 60;

    public GameObject[] notePrefabs;
    public List<Transform> spawnRails;

    // 대기열 느낌으로 이릅 바꾸기
    private List<GameNoteInfo> noteSpawnQueue = new List<GameNoteInfo>();
    // 레일별 대기열으로 이름 바꾸기
    private List<GameNoteInfo>[] noteSpawnQueue_perRail = new List<GameNoteInfo>[railCount];

    // 레입별 게임씬 인스턴스 
    List<EJGameNote>[] spawnedNotes_perRail = new List<EJGameNote>[railCount];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnedNotes_perRail.Length; i++)
        {
            spawnedNotes_perRail[i] = new List<EJGameNote>();
        }

        TestShortNotes();
    }


    void Update()
    {
        currTime += Time.deltaTime;

        for (int i = 0; i < noteSpawnQueue_perRail.Length; i++)
        {
            if (noteSpawnQueue_perRail[i].Count > 0)
            {
                if (currTime >= noteSpawnQueue_perRail[i][0].time / bpm)
                {
                    GameObject prefab = notePrefabs[noteSpawnQueue_perRail[i][0].type];
                    GameObject note = Instantiate(prefab, spawnRails[i].position + Vector3.forward * (-0.5f), prefab.transform.rotation);

                    //note.transform.forward = spawnPos[0].transform.forward;
                    note.transform.SetParent(spawnRails[i].transform);

                    EJGameNote noteInstance = note.GetComponent<EJGameNote>();
                    noteInstantiate(i, noteInstance);
                }
            }
        }
    }

    void noteInstantiate(int n, EJGameNote noteInstance)
    {
        noteInstance.noteInfo = noteSpawnQueue_perRail[n][0];
        spawnedNotes_perRail[n].Add(noteInstance);
        noteSpawnQueue_perRail[n].RemoveAt(0);
    }

    void TestShortNotes()
    {
        // struct로 정의
        GameNoteInfo info = new GameNoteInfo();

        info.railIdx = 0;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1 * bpm;
        noteSpawnQueue.Add(info);

        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 2 * bpm;
        noteSpawnQueue.Add(info);

        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 3 * bpm;
        noteSpawnQueue.Add(info);

        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 4 * bpm;
        noteSpawnQueue.Add(info);

        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 5 * bpm;
        noteSpawnQueue.Add(info);

        info.railIdx = 5;
        info.type = (int)GameNoteType.SHORT;
        info.time = 6 * bpm;
        noteSpawnQueue.Add(info);
        
        for (int i = 0; i < noteSpawnQueue_perRail.Length; i++)
        {
            noteSpawnQueue_perRail[i] = new List<GameNoteInfo>();
        }

        for (int i = 0; i < noteSpawnQueue.Count; i++)
        {
            noteSpawnQueue_perRail[noteSpawnQueue[i].railIdx].Add(noteSpawnQueue[i]);
        }
    }
}
