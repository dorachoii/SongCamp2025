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
    public int DRAG_release_idx;
    public bool isNoteEnabled;
    public byte pitch;

    public NoteData(int railIdx, int type, float time)
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

public class NoteInstance : MonoBehaviour
{
    int bpm = 120;
    public float speed = 5.5f;

    public NoteData noteInfo;
    public GameObject linkNotePrefab;

    GameObject linkNote;
    bool isGrowing = false;

    public Action<int, NoteInstance, bool> autoDestroyAction;
    Transform touchpad;

    public static Func<int, NoteInstance, NoteInstance> GetNextNoteInRail; // delegate injection from NoteManager

    void Start()
    {
        touchpad = GameObject.FindWithTag("TouchPad").transform;

        if ((NoteType)noteInfo.type == NoteType.LONG && noteInfo.isLongNoteStart)
        {
            StartCoroutine(GrowLinkCoroutine());
        }
    }

    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * speed;

        if (transform.position.y + 3f < touchpad.position.y)
        {
            autoDestroy(true);
        }
    }

    IEnumerator GrowLinkCoroutine()
    {
        linkNote = Instantiate(linkNotePrefab, transform.position, Quaternion.identity);

        linkNote.transform.SetParent(transform);
        linkNote.transform.localPosition = Vector3.zero;
        Vector3 scale = linkNote.transform.localScale;
        scale.y = 0;
        linkNote.transform.localScale = scale;

        isGrowing = true;

        NoteInstance endNote = null;

        while (isGrowing)
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
                isGrowing = false;              
            }

            yield return null;
        }
    }

    public void autoDestroy(bool isPassed = false)
    {
        if (autoDestroyAction != null) autoDestroyAction(noteInfo.railIdx, this, isPassed);
        Destroy(gameObject);
    }
}
