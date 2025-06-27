using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSong : MonoBehaviour
{
    public static SampleSong Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // 레일별 리스트 초기화
        for (int i = 0; i < railCount; i++)
        {
            gameNoteInfo_Rails[i] = new List<NoteData>();
        }
    }


    public List<NoteData> allGameNoteInfo = new List<NoteData>();
    public List<NoteData>[] gameNoteInfo_Rails = new List<NoteData>[railCount];
    const int railCount = 6;
    
    //05. FLOP test
    #region FLOP
    public void InputTestFLOP()
    {
        NoteData info = new NoteData();

        #region Pattern01

        //���� 1) Pattern 1 - Short
        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.SHORT;
        info.time = 1;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 31;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 61;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 76;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 5;
        info.type = (int)NoteType.SHORT;
        info.time = 121;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 151;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 181;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 211;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //���� 2) Pattern 1
        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.SHORT;
        info.time = 241;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 271;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 301;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 316;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 5;
        info.type = (int)NoteType.SHORT;
        info.time = 361;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 391;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 421;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 451;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //���� 3) Pattern 1
        info = new NoteData();
        info.railIdx = 0;
        info.type = (int)NoteType.SHORT;
        info.time = 481;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.SHORT;
        info.time = 511;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 541;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.SHORT;
        info.time = 556;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 601;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 631;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 661;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.SHORT;
        info.time = 691;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //���� 4) Pattern 1
        info = new NoteData();
        info.railIdx = 0;
        info.type = (int)NoteType.SHORT;
        info.time = 721;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.SHORT;
        info.time = 751;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 781;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.SHORT;
        info.time = 796;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 841;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 871;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 901;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.SHORT;
        info.time = 931;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);


        #endregion

        #region Pattern02
        //���� 5) Pattern 2
        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 961;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 976;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 983.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 998.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1006;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 1021;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1028.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.LONG;
        info.time = 1043.5f;
        info.isLongNoteStart = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.LONG;
        info.time = 1058.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1081f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 1096f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1103.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1118.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1126f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.LONG;
        info.time = 1141f;
        info.isLongNoteStart = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.LONG;
        info.time = 1178.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //���� 6) Pattern 2
        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1201;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 1216;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1223.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1238.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1246;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 1261;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1305;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1320;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1327.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 1342.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1350;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 5;
        info.type = (int)NoteType.LONG;
        info.time = 1365;
        info.isLongNoteStart = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 5;
        info.type = (int)NoteType.LONG;
        info.time = 1411;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //���� 7) Pattern 2
        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1441;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1456;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1463.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 1478.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1486;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1501;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1508.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.LONG;
        info.time = 1523.5f;
        info.isLongNoteStart = true;
        allGameNoteInfo.Add(info);

        //***
        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.LONG;
        info.time = 1550;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1561;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1576;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1583.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 1598.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1606;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.LONG;
        info.time = 1621;
        info.isLongNoteStart = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.LONG;
        info.time = 1658.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //���� 8) Pattern 2

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1681;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 1696;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1703.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1718.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1726;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 1741;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1785;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1800;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 1807.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 1822.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1830;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.LONG;
        info.time = 1845;
        info.isLongNoteStart = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.LONG;
        info.time = 1891;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        #endregion

        #region Pattern03

        //���� 9) Pattern 3
        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 1921;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 1921;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.SHORT;
        //info.time = 1951;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 1981;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 1981;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 2011;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 2041;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2041;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2071;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.DRAG_RIGHT;
        info.time = 2101;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2101;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2131;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //���� 10) Pattern 3

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 2161;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2161;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 2191;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 2221;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2221;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2251;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.DRAG_RIGHT;
        info.time = 2281;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2281;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2311;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2341;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2363.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 2371;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 2393.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //���� 11) Pattern 3
        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 2401;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2401;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 2431;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 2461;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2461;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2491;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.DRAG_RIGHT;
        info.time = 2521;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2521;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2551;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 2581;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2581;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 2611;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //���� 12) Pattern 3
        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 2641;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2641;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2671;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 2701;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.DRAG_LEFT;
        info.time = 2701;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2731;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 2761;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 2761;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 2791;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 2821;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2843.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 2851;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)NoteType.SHORT;
        info.time = 2873.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);
        #endregion

        #region Pattern04

        //���� 13-14) Pattern 4
        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.LONG;
        info.time = 2881;
        info.isLongNoteStart = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.LONG;
        info.time = 3090;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.LONG;
        info.time = 3091;
        info.isLongNoteStart = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.LONG;
        info.time = 3300;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 3301;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 3323.5f;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 3331;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 3353.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //���� 15-16) Pattern 4

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.LONG;
        info.time = 3361;
        info.isLongNoteStart = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.LONG;
        info.time = 3570;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.LONG;
        info.time = 3571;
        info.isLongNoteStart = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)NoteType.LONG;
        info.time = 3780;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 4;
        //info.type = (int)NoteType.SHORT;
        //info.time = 3781;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.SHORT;
        //info.time = 3803.5f;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = -1;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)NoteType.SHORT;
        info.time = 3811;
        info.isLongNoteStart = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)NoteType.SHORT;
        info.time = 3833.5f;
        info.isLongNoteStart = false;
        info.isLastNote = true;
        allGameNoteInfo.Add(info);

        #endregion


        for (int i = 0; i < gameNoteInfo_Rails.Length; i++)
        {
            gameNoteInfo_Rails[i] = new List<NoteData>();
        }

        for (int i = 0; i < allGameNoteInfo.Count; i++)
        {
            gameNoteInfo_Rails[allGameNoteInfo[i].railIdx].Add(allGameNoteInfo[i]);
        }
    }
    #endregion

    //scoreManager Script�� ����
}
