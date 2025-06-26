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

// startNote생성 되면 noteInstance에서 Connect실행해서 startNote의 자식으로 link와 end가 들어간다.
// startNote위치 기준으로 JudgeTouchedTiming이 실행된다.
//- badZone안으로 들어오면 isHolding이 true가 된다. 그리고 뗀 시점을 체크한다 JudgeReleasingTiming
//- badZone안일때 터치하지않으면 isHolding이 false인 상태고 disabled된다.
// longNote일때는 endNote가 touchpad를 지나면 사라진다.
// 도중에 떼지면 disabled가 된다.



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
    bool isConnecting = false;
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
            linkNote.transform.localScale += new Vector3(0, growSpeed / 3f, 0);

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
