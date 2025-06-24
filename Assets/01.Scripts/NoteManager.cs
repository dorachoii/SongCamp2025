using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    const int railCount = 6;
    float currTime;
    int bpm = 60;

    public GameObject[] notePrefabs;
    public List<Transform> spawnPos;

    private List<GameNoteInfo> allGameNoteInfo = new List<GameNoteInfo>();
    private List<GameNoteInfo>[] gameNoteInfo_Rails = new List<GameNoteInfo>[railCount];

    List<EJGameNote>[] gameNoteInstance_Rails = new List<EJGameNote>[railCount];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameNoteInstance_Rails.Length; i++)
        {
            gameNoteInstance_Rails[i] = new List<EJGameNote>();
        }

        TestShortNotes();
    }


    void Update()
    {
        currTime += Time.deltaTime;

        for (int i = 0; i < gameNoteInfo_Rails.Length; i++)
        {
            if (gameNoteInfo_Rails[i].Count > 0)
            {
                if (currTime >= gameNoteInfo_Rails[i][0].time / bpm)
                {
                    GameObject prefab = notePrefabs[gameNoteInfo_Rails[i][0].type];
                    GameObject note = Instantiate(prefab, spawnPos[i].position + Vector3.forward * (-0.5f), prefab.transform.rotation);

                    //note.transform.forward = spawnPos[0].transform.forward;
                    note.transform.SetParent(spawnPos[i].transform);

                    EJGameNote noteInstance = note.GetComponent<EJGameNote>();
                    noteInstantiate(i, noteInstance);
                }
            }
        }
    }

    void noteInstantiate(int n, EJGameNote noteInstance)
    {
        noteInstance.noteInfo = gameNoteInfo_Rails[n][0];
        gameNoteInstance_Rails[n].Add(noteInstance);
        gameNoteInfo_Rails[n].RemoveAt(0);
    }

    void TestShortNotes()
    {
        // struct로 정의
        GameNoteInfo info = new GameNoteInfo();

        info.railIdx = 0;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1 * bpm;
        allGameNoteInfo.Add(info);

        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 2 * bpm;
        allGameNoteInfo.Add(info);

        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 3 * bpm;
        allGameNoteInfo.Add(info);

        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 4 * bpm;
        allGameNoteInfo.Add(info);

        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 5 * bpm;
        allGameNoteInfo.Add(info);

        info.railIdx = 5;
        info.type = (int)GameNoteType.SHORT;
        info.time = 6 * bpm;
        allGameNoteInfo.Add(info);
        
        for (int i = 0; i < gameNoteInfo_Rails.Length; i++)
        {
            gameNoteInfo_Rails[i] = new List<GameNoteInfo>();
        }

        for (int i = 0; i < allGameNoteInfo.Count; i++)
        {
            gameNoteInfo_Rails[allGameNoteInfo[i].railIdx].Add(allGameNoteInfo[i]);
        }
    }
}
