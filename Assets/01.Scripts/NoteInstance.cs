using System.Collections;
using UnityEngine;
using System;

public enum NoteType
{
    SHORT,
    LONG,
    DRAG_RIGHT,
    DRAG_LEFT,
}

[System.Serializable]
public struct NoteData
{
    public int railIdx;
    public int type;
    public float time;
    public bool isLongNoteStart;
    public byte pitch;

    public NoteData(int railIdx, int type, float time)
    {
        this.railIdx = railIdx;
        this.type = type;
        this.time = time;
        this.isLongNoteStart = false;
        this.pitch = 0;
    }
}

public class NoteInstance : MonoBehaviour
{
    int bpm = 120;
    public float speed = 5.5f;

    public NoteData noteInfo;
    public GameObject linkNotePrefab;
    public Material disabledMat;

    GameObject linkNote;
    public NoteInstance linkedEndNote;


    bool isConnecting = false;
    public bool isJudged = false;
    public bool isEnabled = true;
    public bool isHolding = false;

    Transform touchpad;

    public static Func<int, NoteInstance, NoteInstance> GetNextNoteInRail;

    void OnEnable()
    {
        NoteJudge.OnNoteJudged += OnConfirmed;
    }

    void OnDisable()
    {
        NoteJudge.OnNoteJudged -= OnConfirmed;
    }

    void OnConfirmed(NoteJudgedEventData data)
    {
        if (data.noteInstance == this)
        {
            autoDestroy();
        }

        if (data.noteInstance == linkedEndNote)
        {
            isJudged = true;
            autoDestroy();
        }
    }

    void Start()
    {
        touchpad = GameObject.FindWithTag("TouchPad").transform;

        if ((NoteType)noteInfo.type == NoteType.LONG && noteInfo.isLongNoteStart)
        {
            StartCoroutine(ConnectLinkCoroutine());
        }
    }

    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * speed;

        if ((NoteType)noteInfo.type == NoteType.LONG && noteInfo.isLongNoteStart && linkedEndNote != null)
        {
            if (linkedEndNote.transform.position.y + 3f < touchpad.position.y)
            {
                if (!isHolding)
                {
                    if (!isEnabled)
                    {
                        autoDestroy(false);
                    }
                    else
                    {
                       autoDestroy(true); 
                    }
                }
                else
                {
                    //holding인 상태로 지나갔다면 excellent
                    NoteJudge.NotifyPassed(this);
                }
            }

            if (this.transform.position.y + 3f < touchpad.position.y)
            {
                if (!isHolding && !isJudged)
                {
                    print("isHolding아니고 isJudged아니라서 setDisableVisual");
                    SetDisableVisual();
                }
            }
        }
        else
        {
            if (transform.position.y + 3f < touchpad.position.y)
            {
                if (!isHolding)
                    autoDestroy(true);
            }
        }
    }


    IEnumerator ConnectLinkCoroutine()
    {
        linkNote = Instantiate(linkNotePrefab, transform.position, Quaternion.identity);

        linkNote.transform.SetParent(transform);
        linkNote.transform.localPosition = Vector3.zero;
        Vector3 scale = linkNote.transform.localScale;
        scale.y = 0;
        linkNote.transform.localScale = scale;

        isConnecting = true;

        NoteInstance endNote = null;

        while (isConnecting)
        {
            float growSpeed = speed * Time.deltaTime;
            linkNote.transform.localScale += new Vector3(0, growSpeed / 3f, 0);

            if (endNote == null && GetNextNoteInRail != null)
            {
                endNote = GetNextNoteInRail(noteInfo.railIdx, this);
            }

            if (endNote != null)
            {
                endNote.transform.SetParent(transform);
                endNote.gameObject.GetComponent<NoteInstance>().enabled = false;

                this.linkedEndNote = endNote;
                isConnecting = false;
            }
            yield return null;
        }
    }

    public void autoDestroy(bool isPassed = false)
    {
        if (isPassed)
        {
            NoteJudge.NotifyMiss(this);
        }

        NoteMaker.Instance.spawnedNotes_perRail[noteInfo.railIdx].Remove(this);
        Destroy(gameObject);
    }

    public void SetDisableVisual()
    {
        isEnabled = false;
        var renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers)
        {
            if (renderer != null && disabledMat != null)
            {
                renderer.material = disabledMat;
            }
        }
    }
}
