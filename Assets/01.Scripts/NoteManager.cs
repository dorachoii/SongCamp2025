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

    private List<NoteData> noteSpawnQueue = new List<NoteData>();
    private List<NoteData>[] noteSpawnQueue_perRail = new List<NoteData>[railCount];
    List<NoteInstance>[] spawnedNotes_perRail = new List<NoteInstance>[railCount];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnedNotes_perRail.Length; i++)
        {
            spawnedNotes_perRail[i] = new List<NoteInstance>();
        }

        TestSHORT();
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

                    note.transform.SetParent(spawnRails[i].transform);

                    NoteInstance noteInstance = note.GetComponent<NoteInstance>();
                    noteInstantiate(i, noteInstance);
                }
            }
        }
    }

    void noteInstantiate(int n, NoteInstance noteInstance)
    {
        noteInstance.noteInfo = noteSpawnQueue_perRail[n][0];
        spawnedNotes_perRail[n].Add(noteInstance);
        noteSpawnQueue_perRail[n].RemoveAt(0);
    }

    void TestSHORT()
    {
        NoteData note = new NoteData();

        note.railIdx = 0;
        note.type = (int)GameNoteType.SHORT;
        note.time = 1 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 1;
        note.type = (int)GameNoteType.SHORT;
        note.time = 2 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 2;
        note.type = (int)GameNoteType.SHORT;
        note.time = 3 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 3;
        note.type = (int)GameNoteType.SHORT;
        note.time = 4 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 4;
        note.type = (int)GameNoteType.SHORT;
        note.time = 5 * bpm;
        noteSpawnQueue.Add(note);

        note.railIdx = 5;
        note.type = (int)GameNoteType.SHORT;
        note.time = 6 * bpm;
        noteSpawnQueue.Add(note);
        
        for (int i = 0; i < noteSpawnQueue_perRail.Length; i++)
        {
            noteSpawnQueue_perRail[i] = new List<NoteData>();
        }

        for (int i = 0; i < noteSpawnQueue.Count; i++)
        {
            noteSpawnQueue_perRail[noteSpawnQueue[i].railIdx].Add(noteSpawnQueue[i]);
        }
    }
}
