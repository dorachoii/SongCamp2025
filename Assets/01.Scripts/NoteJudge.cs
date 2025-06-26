using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;

public enum JudgeResult
{
    Miss,
    Bad,
    Good,
    Great,
    Excellent
}

public struct NoteJudgedEventData
{
    public JudgeResult result;
    public int railIndex;
    public NoteType noteType;
    public NoteInstance noteInstance;

    public NoteJudgedEventData(JudgeResult result, int railIndex, NoteType noteType, NoteInstance noteInstance)
    {
        this.result = result;
        this.railIndex = railIndex;
        this.noteType = noteType;
        this.noteInstance = noteInstance;
    }
}

public class NoteJudge : MonoBehaviour
{
    public GameObject[] touchPad;
    List<NoteInstance>[] spawnedNotes_perRail;

    public static event System.Action<NoteJudgedEventData> OnNoteJudged;

    private readonly float badZone = 1.2f;
    private readonly float goodZone = 1f;
    private readonly float greatZone = 0.8f;
    private readonly float excellentZone = 0.6f;

    private void Start()
    {
        spawnedNotes_perRail = NoteMaker.Instance.spawnedNotes_perRail;
    }
    // NoteJudge 클래스 내부에 추가
    public static void NotifyMiss(NoteInstance note)
    {
        var data = new NoteJudgedEventData(JudgeResult.Miss, note.noteInfo.railIdx, (NoteType)note.noteInfo.type, note);
        OnNoteJudged?.Invoke(data);
    }

    public static void NotifyPassed(NoteInstance note)
    {
        var data = new NoteJudgedEventData(JudgeResult.Great, note.noteInfo.railIdx, (NoteType)note.noteInfo.type, note);
        OnNoteJudged?.Invoke(data);
    }

    public void JudgeReleasingTiming(int railIndex, int noteIndex = 0)
    {
        if (spawnedNotes_perRail[railIndex].Count == 0) return;
        var note = spawnedNotes_perRail[railIndex][noteIndex];
        if (note == null) return;

        float dist = note.transform.position.y - touchPad[railIndex].transform.position.y;
        float distAbs = Mathf.Abs(dist);

        JudgeResult result;

        if (distAbs <= excellentZone) result = JudgeResult.Excellent;
        else if (distAbs <= greatZone) result = JudgeResult.Great;
        else if (distAbs <= goodZone) result = JudgeResult.Good;
        else if (distAbs <= badZone) result = JudgeResult.Bad;
        else
        {
            if (note.noteInfo.type == (int)NoteType.LONG)
            {
                note.isEnabled = false;
                note.SetDisableVisual();

                var long_data = new NoteJudgedEventData(JudgeResult.Miss, railIndex, (NoteType)note.noteInfo.type, note);
                OnNoteJudged?.Invoke(long_data);
               
            }
            return;
        }
        var data = new NoteJudgedEventData(result, railIndex, (NoteType)note.noteInfo.type, note);
        OnNoteJudged?.Invoke(data);
        
    }

    public bool JudgeTouchedTiming(int railIndex)
    {
        if (spawnedNotes_perRail[railIndex].Count == 0) return false;

        var note = spawnedNotes_perRail[railIndex][0];
        if (note == null) return false;

        float dist = note.transform.position.y - touchPad[railIndex].transform.position.y;
        float distAbs = Mathf.Abs(dist);

        return distAbs <= badZone;
    }
}
