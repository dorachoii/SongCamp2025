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

    public void JudgeDragNote(bool isRight)
{
    int[] targetRails = isRight ? new int[] { 3, 4, 5 } : new int[] { 2, 1, 0 };
    string[] expectedKeys = isRight ? new string[] { "j", "k", "l" } : new string[] { "f", "d", "s" };

    // 1. DRAG 노트 존재 여부 및 위치 판정
    NoteInstance firstNote = spawnedNotes_perRail[targetRails[0]].Count > 0 ?
                            spawnedNotes_perRail[targetRails[0]][0] : null;

    if (firstNote == null)
    {
        Debug.Log("JudgeDragNote: 해당 레일에 노트가 없습니다.");
        return;
    }

    NoteType noteType = (NoteType)firstNote.noteInfo.type;
    if (noteType != (isRight ? NoteType.DRAG_RIGHT : NoteType.DRAG_LEFT))
    {
        Debug.Log($"JudgeDragNote: 노트 타입 불일치. 기대: {(isRight ? NoteType.DRAG_RIGHT : NoteType.DRAG_LEFT)}, 실제: {noteType}");
        return;
    }

    float dist = firstNote.transform.position.y - touchPad[targetRails[0]].transform.position.y;
    if (Mathf.Abs(dist) > badZone)
    {
        Debug.Log($"JudgeDragNote: 거리 초과. 현재 거리: {Mathf.Abs(dist)}, 허용 범위: {badZone}");
        return;
    }

    // 2. 키 입력 체크
    bool correctOrder = true;
    float[] keyTimes = new float[3];

    for (int i = 0; i < 3; i++)
    {
        if (!Input.GetKeyDown(expectedKeys[i]))
        {
            Debug.Log($"JudgeDragNote: 예상 키 '{expectedKeys[i]}' 가 눌리지 않았습니다.");
            correctOrder = false;
            break;
        }
        keyTimes[i] = Time.time;
        Debug.Log($"JudgeDragNote: '{expectedKeys[i]}' 눌림, 시간: {keyTimes[i]}");
    }

    // 3. 시간 간격 판정 (0.2초 내)
    if (correctOrder)
    {
        float interval = keyTimes[2] - keyTimes[0];
        if (interval <= 0.2f)
        {
            Debug.Log($"JudgeDragNote: 성공! 키 입력 간격 {interval}초");
            OnNoteJudged?.Invoke(JudgeResult.Excellent, targetRails[0]);
        }
        else
        {
            Debug.Log($"JudgeDragNote: 시간 초과. 입력 간격: {interval}초");
        }
    }
}


}
