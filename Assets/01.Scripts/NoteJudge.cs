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

    public static event System.Action<JudgeResult, int> OnNoteJudged;

    private readonly float badZone = 0.8f;
    private readonly float goodZone = 0.6f;
    private readonly float greatZone = 0.5f;
    private readonly float excellentZone = 0.4f;

    private void Start()
    {
        spawnedNotes_perRail = NoteManager.Instance.spawnedNotes_perRail;
    }
    // NoteJudge 클래스 내부에 추가
    public static void NotifyMiss(int railIdx)
    {
        OnNoteJudged?.Invoke(JudgeResult.Miss, railIdx);
    }


    public void JudgeShortNote(int railIndex)
    {
        if (spawnedNotes_perRail[railIndex].Count == 0) return;
        var note = spawnedNotes_perRail[railIndex][0];
        if (note == null) return;

        float dist = note.transform.position.y - touchPad[railIndex].transform.position.y;
        float distAbs = Mathf.Abs(dist);

        JudgeResult result;

        if (distAbs <= excellentZone) result = JudgeResult.Excellent;
        else if (distAbs <= greatZone) result = JudgeResult.Great;
        else if (distAbs <= goodZone) result = JudgeResult.Good;
        else if (distAbs <= badZone) result = JudgeResult.Bad;
        else return;

        OnNoteJudged?.Invoke(result, railIndex);
    }

    private int dragStep = 0;
    private float dragStartTime = -1f;

    

}
