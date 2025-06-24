using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;

public enum JudgeResult
{
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

    public void JudgeShortNote(int railIndex)
    {
        if (spawnedNotes_perRail[railIndex].Count == 0) return;
        print($"판정: 누른 레일 {railIndex}, 현재 레일에 생성된 노트의 수: {spawnedNotes_perRail[railIndex].Count}");
        var note = spawnedNotes_perRail[railIndex][0];

        float dist = note.transform.position.y - touchPad[railIndex].transform.position.y;
        float distAbs = Mathf.Abs(dist);

        JudgeResult result;
        print($"판정: 거리: {distAbs}");

        if (distAbs <= excellentZone) result = JudgeResult.Excellent;
        else if (distAbs <= greatZone) result = JudgeResult.Great;
        else if (distAbs <= goodZone) result = JudgeResult.Good;
        else if (distAbs <= badZone) result = JudgeResult.Bad;
        else return;

        print($"판정: 거리: {distAbs}, 결과: {result}");

        OnNoteJudged?.Invoke(result, railIndex);
    }
}
