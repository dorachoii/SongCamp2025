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

    //longNote일때 start이면 true, end이면 false
    public bool isLongNoteStart;

    //dragNote가 떼져야 하는 index
    public int DRAG_release_idx;

    //longNote나 dragNote가 눌리다가 말아서 enable체크를 해야할 때
    public bool isNoteEnabled;

    public byte pitch; 
}

public class MidiEventInfo
{
    public byte pitch;      //계이름
    public float length;    //음 길이

    public float startTime; //시작 시간이 필요함
    public float endTime;    
}

public class EJGameNoteStorage : MonoBehaviour
{

}

