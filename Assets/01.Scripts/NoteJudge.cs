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

public class NoteJudge : MonoBehaviour
{
    public GameObject[] touchPad;
    List<NoteInstance>[] spawnedNotes_perRail;

    public static event System.Action<JudgeResult, int, NoteType> OnNoteJudged;
    public static event System.Action<NoteInstance> OnNoteConfirmed;

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
        OnNoteJudged?.Invoke(JudgeResult.Miss, note.noteInfo.railIdx, (NoteType)note.noteInfo.type);
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
        else return;

        OnNoteJudged?.Invoke(result, railIndex, (NoteType)note.noteInfo.type);
        OnNoteConfirmed?.Invoke(note);
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
