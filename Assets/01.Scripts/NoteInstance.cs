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
    GameObject linkNote;
    bool isConnecting = false;
    public bool isEnabled = true;
    public bool isHolding = false;

    public static event Action<NoteInstance> OnNoteDestroyed;
    Transform touchpad;

    public static Func<int, NoteInstance, NoteInstance> GetNextNoteInRail;

    void OnEnable()
    {
        NoteJudge.OnNoteConfirmed += OnConfirmed;        
    }

    void OnDisable()
    {
        NoteJudge.OnNoteConfirmed -= OnConfirmed; 
    }

    void OnConfirmed(NoteInstance note)
    {
        if (note == this)
        {
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

        if (transform.position.y + 3f < touchpad.position.y)
        {
            if (isHolding) return;
            autoDestroy(true);
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
            linkNote.transform.localScale += new Vector3(0, growSpeed / 2.5f, 0);

            if (endNote == null && GetNextNoteInRail != null)
            {
                endNote = GetNextNoteInRail(noteInfo.railIdx, this);

            }

            if (endNote != null)
            {
                endNote.transform.SetParent(transform);
                endNote.gameObject.GetComponent<NoteInstance>().enabled = false;
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
        OnNoteDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
