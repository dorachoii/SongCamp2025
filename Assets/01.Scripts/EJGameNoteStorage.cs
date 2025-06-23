using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

//Note Storage

//01. GameNoteType
//02. GameNoteInfo

//03. MidiNoteInfo

//All Notes
public enum GameNoteType
{
    SHORT,
    LONG,
    DRAG_RIGHT,
    DRAG_LEFT,
    DRAG_empty
}

[System.Serializable]
public struct GameNoteInfo
{
    public int railIdx;
    public int type;
    public float time;

    //longNote�϶� start�̸� true, end�̸� false
    public bool isLongNoteStart;

    //dragNote�� ������ �ϴ� index
    public int DRAG_release_idx;

    //longNote�� dragNote�� �����ٰ� ���Ƽ� enableüũ�� �ؾ��� ��
    public bool isNoteEnabled;

    public byte pitch; 
}

public class MidiEventInfo
{
    public byte pitch;      //���̸�
    public float length;    //�� ����

    public float startTime; //���� �ð��� �ʿ���
    public float endTime;    
}

public class EJGameNoteStorage : MonoBehaviour
{

}

