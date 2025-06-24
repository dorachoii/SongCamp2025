using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameNoteType
{
    SHORT,
    LONG,
    DRAG_RIGHT,
    DRAG_LEFT,
}

// gamenote 골격
[System.Serializable]
public struct GameNoteInfo
{
    public int railIdx;
    public int type;
    public float time;
    public bool isLongNoteStart;
    public int DRAG_release_idx;
    public bool isNoteEnabled;
    public byte pitch;

    // 생성자
    public GameNoteInfo(int railIdx, int type, float time)
    {
        this.railIdx = railIdx;
        this.type = type;
        this.time = time;

        this.isLongNoteStart = false;
        this.DRAG_release_idx = 0;
        this.isNoteEnabled = true;
        this.pitch = 0;
    }
}


// gamescene에 인스턴스화된 클래스
//01.Note_Flow
//02.Note_Connect
//03.Note_autoDestroy
    
public class EJGameNote : MonoBehaviour
{
    //01.Note_Flow Variables
    int bpm = 120;
    float spb;
    public float speed = 5.5f;

    //02.Note_Connect Variables
    public GameNoteInfo noteInfo;
    public GameObject linkNotePrefab;
    GameObject linkNote;
    GameObject startN;
    GameObject endN;

    //03.Note_autoDestroy
    public Action<int, EJGameNote, bool> autoDestroyAction;
    Transform touchpad;


    void Start()
    {
        //01.Note_Flow
        spb = 60 / bpm;

        //02.Note_autoDestroy
        touchpad = GameObject.FindWithTag("TouchPad").transform;
    }


    void Update()
    {
        //01.Note_Flow
        transform.position += Vector3.down * Time.deltaTime * speed;

        //02.Note_autoDestroy isPassed Check
        if (transform.position.y + 3f < touchpad.position.y)
        {
            autoDestroy(true);
        }

    }

    //02.Note_autoDestroy
    public void autoDestroy(bool isPassed = false)
    {
        //autoDestroyAction Parameter
        //01. rail_idx
        //02. noteInfo
        //03. passDestroy

        if (autoDestroyAction != null) autoDestroyAction(noteInfo.railIdx, this, isPassed);
        Destroy(gameObject);
    }

    //03.Note_Connect
    //03-1.endNote�� ������ start-end������ �Ÿ��� ���� �̾��ִ� ���
    public void connectNote(GameObject endN)
    {
        print("* connectNote�� ����Ǿ����ϴ�");

        startN = this.gameObject;

        if (endN == null) return;

        linkNote = Instantiate(linkNotePrefab, (startN.transform.position + endN.transform.position) / 2, Quaternion.identity);
        linkNote.transform.SetParent(endN.transform);

        float length = (endN.transform.localPosition.y - startN.transform.localPosition.y);
        linkNote.transform.localScale += new Vector3(0, length, 0);
    }


    //03-2.startNote�� ������ linkNote�� �Ÿ��� �ø��ٰ�, end�� ������ �ڶ��� �ʴ� ���
    public IEnumerator connectNote2(GameObject endN)
    {
        linkNote = Instantiate(linkNotePrefab, (startN.transform.position + endN.transform.position) / 2, Quaternion.identity);
        linkNote.transform.SetParent(startN.transform);
        float length = (endN.transform.localPosition.y - startN.transform.localPosition.y);

        while (linkNote.transform.position.y == length)
        {
            linkNote.transform.position += new Vector3(0, 1, 0);
            yield return null;
        }
    }

    public void startConnecting(GameObject endN)
    {
        StartCoroutine(connectNote2(endN));
    }
}
