using Melanchall.DryWetMidi.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

using UnityEngine;

//01. Note_Instantiate & Destroy
//02. scoreCheck

public class EJNoteManager : MonoBehaviour
{
    //����
    int bpm = 80;
    public Camera maincam;

    //01. Note_Instantiate
    public GameObject[] notePrefabs;
    public Transform[] noteSpawnRail;
    public Transform[] touchpads;

    //Touch�� �����ؼ� ���� �� ��
    public GameObject[] QuadRails;
    public GameObject[] QuadTouches;
    int[] pitches_rail = new int[6];

    public GameObject touchFX;
    public GameObject touchfireFX;
    public GameObject fireworkFX;

    public AudioSource[] audiosource;
    public AudioClip[] SFXs;

    public Transform[] touchFX_pos;
    public Transform[] touchfireFX_pos;
    public Transform[] fireworkFX_pos;

    public Material[] QuadRails_Mat;

    GameObject note;
    GameObject startNote;
    GameObject endNote;

    const int railCount = 6;
    float currTime;

    //01-1.noteData _ ������ ��⿭ ����
    public List<NoteData> allGameNoteInfo = new List<NoteData>();
    public List<NoteData>[] gameNoteInfo_Rails = new List<NoteData>[railCount];

    //01-2.Hierarchy - instance noteData
    List<NoteInstance>[] gameNoteInstance_Rails = new List<NoteInstance>[railCount];
    NoteInstance[] gameStartNoteArr = new NoteInstance[railCount];

    //02. Note_pressCheck
    bool[] isTouchPadPressed = new bool[railCount];
    bool[] isDragPressed = new bool[railCount];
    int touchStartedIdx;
    int touchReleasedIdx;

    enum DraggingState
    {
        None,
        Dragging_RIGHT,
        Dragging_LEFT,
    }

    DraggingState draggingState;

    Touch touch;
    TouchPhase phase;
    Vector2 deltaPos;

    public Material missMat;

    //03. scoreCheck;
    float distAbs;     //touchPad�� note������ �Ÿ� üũ
    float dist;

    float badZone = 2.9f;
    float goodZone = 2f;
    float greatZone = 1f;
    float excellentZone = 0.3f;

    int badScore = 1;
    int goodScore = 2;
    int greatScore = 3;
    int excellentScore = 5;
    int missScore = -1;

    float pressScore = 1;

    public Canvas canvas;
    public GameObject[] scoreTexts;

    void Start()
    {

        // instantiated note in hierarchy <<< Add EJNote Component 
        for (int i = 0; i < gameNoteInstance_Rails.Length; i++)
        {
            //notes properties list per Rails
            gameNoteInstance_Rails[i] = new List<NoteInstance>();
        }

        InputTestSHORTNotes();    //test FINISHED!!!
        //InputTestLONGNotes();     //test FINISHED_1��!!!
        //InputTestDRAGNote();
        //InputTestMIXEDNote();
        //InputTestFLOP();

        //StartCoroutine(Test());

        

    }

    // IEnumerator Test()
    // {
    //     yield return new WaitForSeconds(0.2f);
    //     MIDIPlayer.instance.PlayOneMidiEvent(gameNoteInstance_Rails[0][0].noteInfo.pitch);
    //     yield return new WaitForSeconds(0.2f);
    //     MIDIPlayer.instance.PlayOneMidiEvent(gameNoteInstance_Rails[1][0].noteInfo.pitch);
    //     yield return new WaitForSeconds(0.2f);
    //     MIDIPlayer.instance.PlayOneMidiEvent(gameNoteInstance_Rails[2][0].noteInfo.pitch);
    //     yield return new WaitForSeconds(0.2f);
    //     MIDIPlayer.instance.PlayOneMidiEvent(gameNoteInstance_Rails[4][0].noteInfo.pitch);
    //     yield return new WaitForSeconds(0.2f);
    //     MIDIPlayer.instance.PlayOneMidiEvent(gameNoteInstance_Rails[5][0].noteInfo.pitch);
    // }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //StartCoroutine(finaleFX());
            //StartCoroutine(fireworkkFX());

            startcoFinaleFX();
        }



        currTime += Time.deltaTime;

        //01. Note_Instantiate & Destroy    //test FINISHED!!!
        #region 01. Note_Instantiate & Destroy

        //01-1. Note_Instantiate
        //note instantiate per rails

        //0~5���� �ݺ��ϸ鼭 ex) 0�� ������ ��
        for (int i = 0; i < gameNoteInfo_Rails.Length; i++)
        {
            // ex) 0�� ���Ͽ� ������� ��Ʈ�� �ִٸ�
            // i = railIndex üũ ��
            if (gameNoteInfo_Rails[i].Count > 0)
            {
                //Note_Instantiate on Time
                //��⿭�� �ִ� 0�� ������ 0�� ��Ʈ�� �����ð��� ����
                //if (currTime >= noteInfo_Rails[i][0].time)
                if (currTime >= gameNoteInfo_Rails[i][0].time / bpm)
                {
                    //Note_Instantiate by NoteType, SpawnRail
                    //01-1-1.NoteType
                    //notePrefabs[type], noteSpawnRail[0],
                    note = Instantiate(notePrefabs[gameNoteInfo_Rails[i][0].type], noteSpawnRail[i].position + Vector3.forward * (-0.5f), Quaternion.identity);

                    note.transform.forward = notePrefabs[0].transform.forward;
                    note.transform.SetParent(noteSpawnRail[i].transform);

                    //���� instantiated�� Note�� info�� ��⿭�� ������ ����ְ�
                    //���ο� ����Ʈ�� �迭�� �־��ְ� ����.
                    NoteInstance noteInstance = note.GetComponent<NoteInstance>();

                    #region �Լ��� ������ �κ� �ȵǸ� Ǯ��
                    //noteInstance.noteInfo = noteInfo_Rails[i][0];
                    //noteInstance_Rails[i].Add(noteInstance);
                    ////Instantiated�Ǹ� ��⿭���� �����ֱ�
                    //noteInfo_Rails[i].RemoveAt(0);
                    #endregion

                    noteInstantiate(i, noteInstance);

                    //01-1-2.NoteType_LONG
                    //LONG�̶�� endNote�� ����
                    if (noteInstance.noteInfo.type == (int)GameNoteType.LONG)
                    {
                        if (noteInstance.noteInfo.isLongNoteStart)
                        {
                            print("*00000 noteInstantiate ���� - noteInstance�� type��" + noteInstance.noteInfo.type + "noteInstance�� isLongStart��" + noteInstance.noteInfo.isLongNoteStart + "���翭�� 0���� ��� ����" + gameNoteInstance_Rails[i][0]);

                            gameStartNoteArr[i] = noteInstance;
                            //startNote = firstNoteInstance.gameObject;
                        }
                        else
                        {
                            print("*11111 noteInstantiate ���� - noteInstance�� type��" + noteInstance.noteInfo.type + "noteInstance�� isLongStart��" + noteInstance.noteInfo.isLongNoteStart + "���翭�� 0���� ��� ����" + gameNoteInstance_Rails[i][0]);

                            int startNoteIdx = gameNoteInstance_Rails[i].Count - 1 - 1;
                            gameNoteInstance_Rails[i][startNoteIdx].GetComponent<NoteInstance>().connectNote(noteInstance.gameObject);
                            
                            //������ startNote ĭ�� �����ش�.
                            //�׷��� endNote�� 0��° �ε����� üũ�� �� �����ϱ�
                            //noteInstance_Rails[i].RemoveAt(0);
                            //noteRemove(i);
                            print("*22222 noteInstantiate ���� - noteInstance�� type��" + noteInstance.noteInfo.type + "noteInstance�� isLongStart��" + noteInstance.noteInfo.isLongNoteStart + "���翭�� 0���� ��� ���� isLongNoteStart��" + gameNoteInstance_Rails[i][0].noteInfo.isLongNoteStart);
                        }
                    }


                    //01-2. Note_AutoDestroy
                    //print("*55555 noteInstance�� enable���´�" + noteInstance.noteInfo.isNoteEnabled);

                    noteInstance.autoDestroyAction = (railIdx, noteInfo, isPassed) =>
                    {
                        //Pass without Press
                        if (isPassed) isTouchPadPressed[railIdx] = false;
                        //Pass >> remove from List
                        //
                        gameNoteInstance_Rails[railIdx].Remove(noteInfo);
                        

                        //!!!!!autoDestroy�� �� ��, �ش� ������ ���� �÷��̵ǰ� �ִٸ� ����.


                        //if (noteInstance.noteInfo.type == (int)NoteType.LONG && !noteInstance.noteInfo.isLongNoteStart && noteInstance.noteInfo.isNoteEnabled) return;

                        if (noteInstance.noteInfo.isNoteEnabled)
                        {
                            //LongNote�� �����ϰ��� ��� �����ִ� ���� ������ miss�� �ƴ�!
                            if (noteInstance.noteInfo.type == (int)GameNoteType.LONG && !noteInstance.noteInfo.isLongNoteStart)
                            {

                            }
                            else
                            {
                                print("*****���� autoDestroyAction�� ��� ����Ǵ� Note�� isLongStart����" + noteInstance.noteInfo.isLongNoteStart);
                                //showScoreText(4);
                                ScoreManager.instance.StartShowScoreText("Miss",railIdx,0);
                                CamShake.instance.StartShake(0.2f, 0.5f, 1);
                            }
                        }
                    };




                }
            }
        }
        #endregion


        //02-1.scoreCheck
        #region scoreCheck by touchPhase

#if UNITY_EDITOR //check FINISHED!!!
        //if (Input.touchCount > 0)
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            //�̰ɷ� �ٽ� �����
            //Drag 0�� ������ ó������ began�ǰ� ���������� �������� üũ�ؾ���.
            //�����ʰ� �ٸ� ��ư�� �����ٸ� �� ��ư�� ��������� �ؾ���.

            if (/*touch.phase == TouchPhase.Began*/Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, 100f, 1 << LayerMask.NameToLayer("touchPad")))
                {
                    //TouchPad ��ȣ Ȯ��
                    string touchPadName = hitInfo.transform.name;
                    touchPadName = touchPadName.Replace("Touch0", "");
                    int touchIdx = int.Parse(touchPadName) - 1;

                    touchStartedIdx = touchIdx;

                    dicCurrTouchPadIdx[0] = -1;
                    touchedFX(touchIdx, 0);

                    if (gameNoteInstance_Rails[touchIdx].Count > 0)
                    {
                        //NoteType Ȯ��
                        if (gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.SHORT)
                        {
                            ScoreCheck_SHORT(touchIdx);
                        }
                        else if (gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.LONG && gameNoteInstance_Rails[touchIdx][0].noteInfo.isLongNoteStart)
                        {
                            EnterCheck_LONG(touchIdx);
                        }
                        else if (gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.DRAG_RIGHT || gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.DRAG_LEFT)
                        {
                            EnterCheck_DRAG(touchIdx);
                        }
                    }
                }
            }   //check FINISHED!!!

            if (/*touch.phase == TouchPhase.Moved*/Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Ray ray = Camera.main.ScreenPointToRay(touch);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, 100f, 1 << LayerMask.NameToLayer("touchPad")))
                {
                    string touchPadName = hitInfo.transform.name;
                    touchPadName = touchPadName.Replace("Touch0", "");
                    int touchIdx = int.Parse(touchPadName) - 1;

                    touchedFX(touchIdx, 0);

                    if (gameNoteInstance_Rails[touchStartedIdx].Count > 0)
                    {
                        if (touchStartedIdx != touchIdx)
                        {
                            print("���� üũ �� touchId��" + touchIdx + "touchStartedIdx��" + touchStartedIdx);

                            //���� üũ
                            if (touchIdx < touchStartedIdx)     //���� �巡��
                            {
                                draggingState = DraggingState.Dragging_LEFT;

                                if (gameNoteInstance_Rails[touchStartedIdx].Count > 0 &&
                                    gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.type == (int)GameNoteType.DRAG_LEFT)
                                {
                                    print("���� idx��" + touchIdx + "�̰�" + " startedIdx��" + touchStartedIdx);
                                    //print("�������� �巡�׵ǰ� �ֽ��ϴ�");

                                    //deltaPos�� ������������ üũ�ϱ�! ���� �������� �����̰� �ִ���
                                    PressingScore(touchStartedIdx);
                                    //showScoreText(9);
                                }
                            }
                            else    //������ �巡��
                            {
                                draggingState = DraggingState.Dragging_RIGHT;

                                if (gameNoteInstance_Rails[touchStartedIdx].Count > 0 &&
                                    gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.type == (int)GameNoteType.DRAG_RIGHT)
                                {
                                    //print("���������� �巡�׵ǰ� �ֽ��ϴ�");
                                    PressingScore(touchStartedIdx);
                                    //showScoreText(10);
                                }
                            }
                        }
                        else //���� ��ư�� �� ������ ��   checkFINISHED !!!
                        {
                            draggingState = DraggingState.None;

                            if (gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.LONG)
                            {
                                PressingScore(touchIdx);
                                //print("*55666 LongNote�� ������ �ֽ��ϴ�");
                                //showScoreText(7);
                            }
                        }
                    }
                }
            }  //check FINISHED!!!

            if (/*touch.phase == TouchPhase.Ended*/Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Ray ray = Camera.main.ScreenPointToRay(touch);
                RaycastHit hitInfo;

                releasedFX(0);
                dicCurrTouchPadIdx.Remove(0);

                if (Physics.Raycast(ray, out hitInfo, 100f, 1 << LayerMask.NameToLayer("touchPad")))
                {
                    string touchPadName = hitInfo.transform.name;
                    touchPadName = touchPadName.Replace("Touch0", "");
                    int touchIdx = int.Parse(touchPadName) - 1;

                    //�� ���� pad ��ȣ Ȯ��
                    touchReleasedIdx = touchIdx;

                    if (gameNoteInstance_Rails[touchStartedIdx].Count > 0)
                    {
                        if (touchStartedIdx != touchReleasedIdx)
                        {
                            if (gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.type == (int)GameNoteType.DRAG_RIGHT || gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.type == (int)GameNoteType.DRAG_LEFT)
                            {
                                ExitCheck_DRAG(touchStartedIdx);
                            }
                        }
                        else
                        {
                            if (gameNoteInstance_Rails[touchIdx].Count > 0 && gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.LONG && !gameNoteInstance_Rails[touchIdx][0].noteInfo.isLongNoteStart)
                            {
                                ExitCheck_LONG(touchIdx);
                            }
                        }
                    }
                }
            }   //check FINISHED!!!

        }
#endif


        if (Input.touchCount > 0)
        {
            print("���� touchCount��" + Input.touchCount);

            for (int i = 0; i < Input.touchCount; i++)
            {
                touch = Input.GetTouch(i);
                //touch.fingerId = i;

                if (touch.phase == TouchPhase.Began)
                {

                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, 100f, 1 << LayerMask.NameToLayer("touchPad")))
                    {
                        //TouchPad ��ȣ Ȯ��
                        string touchPadName = hitInfo.transform.name;
                        touchPadName = touchPadName.Replace("Touch0", "");
                        int touchIdx = int.Parse(touchPadName) - 1;

                        touchStartedIdx = touchIdx;

                        dicCurrTouchPadIdx[touch.fingerId] = -1;
                        touchedFX(touchIdx, touch.fingerId);

                        print(i + "��° touch�� ��" + touchIdx + "���� ��ġ�е尡 ���Ȱ�" + "touchedFX �Լ��� ����Ǿ���.");

                        if (gameNoteInstance_Rails[touchIdx].Count > 0)
                        {
                            //NoteType Ȯ��
                            if (gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.SHORT)
                            {
                                ScoreCheck_SHORT(touchIdx);
                            }
                            else if (gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.LONG && gameNoteInstance_Rails[touchIdx][0].noteInfo.isLongNoteStart)
                            {
                                EnterCheck_LONG(touchIdx);
                            }
                            else if (gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.DRAG_RIGHT || gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.DRAG_LEFT)
                            {
                                EnterCheck_DRAG(touchIdx);
                            }
                        }
                    }
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, 100f, 1 << LayerMask.NameToLayer("touchPad")))
                    {
                        string touchPadName = hitInfo.transform.name;
                        touchPadName = touchPadName.Replace("Touch0", "");
                        int touchIdx = int.Parse(touchPadName) - 1;

                        touchedFX(touchIdx, touch.fingerId);

                        if (gameNoteInstance_Rails[touchStartedIdx].Count > 0)
                        {
                            if (touchStartedIdx != touchIdx)
                            {
                                //���� üũ
                                if (touchIdx < touchStartedIdx)     //���� �巡��
                                {
                                    draggingState = DraggingState.Dragging_LEFT;

                                    if (gameNoteInstance_Rails[touchStartedIdx].Count > 0 &&
                                        gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.type == (int)GameNoteType.DRAG_LEFT)
                                    {
                                        print("���� idx��" + touchIdx + "�̰�" + " startedIdx��" + touchStartedIdx);
                                        print("�������� �巡�׵ǰ� �ֽ��ϴ�");

                                        //deltaPos�� ������������ üũ�ϱ�! ���� �������� �����̰� �ִ���
                                        PressingScore(touchStartedIdx);
                                        //showScoreText(9);
                                    }
                                }
                                else    //������ �巡��
                                {
                                    draggingState = DraggingState.Dragging_RIGHT;

                                    if (gameNoteInstance_Rails[touchStartedIdx].Count > 0 &&
                                        gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.type == (int)GameNoteType.DRAG_RIGHT)
                                    {
                                        print("���������� �巡�׵ǰ� �ֽ��ϴ�");
                                        PressingScore(touchStartedIdx);
                                        //showScoreText(10);
                                    }
                                }
                            }
                            else //���� ��ư�� �� ������ ��   checkFINISHED !!!
                            {
                                draggingState = DraggingState.None;

                                if (gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.LONG)
                                {
                                    PressingScore(touchIdx);
                                    //showScoreText(7);
                                }
                            }
                        }
                    }

                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hitInfo;

                    releasedFX(touch.fingerId);
                    dicCurrTouchPadIdx.Remove(touch.fingerId);

                    if (Physics.Raycast(ray, out hitInfo, 100f, 1 << LayerMask.NameToLayer("touchPad")))
                    {
                        string touchPadName = hitInfo.transform.name;
                        touchPadName = touchPadName.Replace("Touch0", "");
                        int touchIdx = int.Parse(touchPadName) - 1;

                        //�� ���� pad ��ȣ Ȯ��
                        touchReleasedIdx = touchIdx;

                        if (gameNoteInstance_Rails[touchStartedIdx].Count > 0)
                        {
                            if (touchStartedIdx != touchReleasedIdx)
                            {
                                if (gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.type == (int)GameNoteType.DRAG_RIGHT || gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.type == (int)GameNoteType.DRAG_LEFT)
                                {
                                    ExitCheck_DRAG(touchStartedIdx);
                                }
                            }
                            else
                            {
                                if (gameNoteInstance_Rails[touchIdx].Count > 0 && gameNoteInstance_Rails[touchIdx][0].noteInfo.type == (int)GameNoteType.LONG && !gameNoteInstance_Rails[touchIdx][0].noteInfo.isLongNoteStart)
                                {
                                    ExitCheck_LONG(touchIdx);
                                }
                            }
                        }
                    }

                }
            }
        }


        #endregion
    }


    void noteInstantiate(int n, NoteInstance noteInstance)
    {
        //���� ����Ʈ�� ��⿭�� ������ �־ Insert
        noteInstance.noteInfo = gameNoteInfo_Rails[n][0];
        gameNoteInstance_Rails[n].Add(noteInstance);
        //��⿭���� Remove
        gameNoteInfo_Rails[n].RemoveAt(0);
    }

    void noteRemove(int n)
    {
        //���� ����Ʈ���� Remove
        gameNoteInstance_Rails[n].RemoveAt(0);
    }

    void noteUnable(int n)
    {
        //���� ����Ʈ�� unable
        gameNoteInstance_Rails[n][0].noteInfo.isNoteEnabled = false;

        //���� ����Ʈ���� remove
        //noteInstance_Rails[n].RemoveAt(0);
    }

    //���� touch�� �κи� ������ �������� �������� check!!!
    //int currTouchPadIdx = -1;

    //Dictionary<fingerID, railIdx>
    Dictionary<int, int> dicCurrTouchPadIdx = new Dictionary<int, int>();

    //fingerId�� startIdx�� �ʿ�    
    //*****Dictionary<fingerID, startIdx>
    Dictionary<int, int> dicStartIdx = new Dictionary<int, int>();


    
    //pitch�� Rail�� �����

    void touchedFX(int n, int fingerId)
    {
        //n = railIdx
        //fingerId = touchpad

        print("touchTEST: touchedFX�� ����Ǿ��� ����� ������" + n + "�̴�.");

        if (dicCurrTouchPadIdx.ContainsKey(fingerId) == false) return;
        if (n == dicCurrTouchPadIdx[fingerId]) return;

        if (gameNoteInstance_Rails[n].Count > 0)
        {
            pitches_rail[n] = gameNoteInstance_Rails[n][0].noteInfo.pitch;
            
        }

        if (dicCurrTouchPadIdx[fingerId] != -1)
        {
            releasedFX(fingerId);
        }

        if (/*!touchpads[n].GetComponent<MeshRenderer>().enabled &&*/ !QuadTouches[n].activeSelf)
        {
            /*touchpads[n].GetComponent<MeshRenderer>().enabled = true;*/
            QuadTouches[n].SetActive(true);
            QuadRails[n].GetComponent<MeshRenderer>().material = QuadRails_Mat[n];

            GameObject touchEffect = Instantiate(touchFX, touchFX_pos[n]);
            Destroy(touchEffect, 1);

            
            

          //  MIDIPlayer.instance.NoteOn(pitches_rail[n]);


            //on ���ְ� �ش����� �� �� off���ְ�
            //MIDIPlayer.instance.PlayOneMidiEvent();
        }

        dicCurrTouchPadIdx[fingerId] = n;
    }

    
    void releasedFX(int fingerId)
    {
        print("touchTEST:  releasedFX�� ����Ǿ��� ����� fingerId" + fingerId  + "�̴�.");


        if (dicCurrTouchPadIdx.ContainsKey(fingerId) == false) return;

        int n = dicCurrTouchPadIdx[fingerId];
        print("releasedFX�Լ� ����");

        if (n == -1) return;

        if (/*touchpads[n].GetComponent<MeshRenderer>().enabled &&*/ QuadTouches[n].activeSelf)
        {
            /*touchpads[n].GetComponent<MeshRenderer>().enabled = false;*/
            QuadTouches[n].SetActive(false);
            QuadRails[n].GetComponent<MeshRenderer>().material = QuadRails_Mat[n+6];

         //   MIDIPlayer.instance.NoteOff(pitches_rail[n]);
            print("touchTEST:  releasedFX�� ����Ǿ��� NoteOFF�� ����Ǿ���.");
        }

        if (gameNoteInstance_Rails[n].Count == 0)
        {
            pitches_rail[n] = 20;
        }
    }

    int finaleCount = 30;
    int fireworkCount = 15;
    int n = 0;
    int k = 0;
    int dir = 1;

    public IEnumerator finaleFX()
    {
        for (int i = 0; i < finaleCount; i++)
        {
            n += dir;

            print("n��" + n);

            if (n == 5)
            {
                dir *= -1;
                                   
            }
            
            if (n == 0)
            {
                dir *= -1;
                
            }
                GameObject fx = Instantiate(touchfireFX, touchfireFX_pos[n]);
            //audiosource.PlayOneShot(SFXs[0]);
            yield return new WaitForSeconds(0.3f);                             
        }
        n = 0;
    }


    public IEnumerator fireworkkFX()
    {
        for (int i = 0; i < fireworkCount; i++)
        {
            k += dir;

            print("��" + n);

            if (k == 2)
            {
                dir *= -1;

            }

            if (k == 0)
            {
                dir *= -1;

            }

            GameObject fx = Instantiate(fireworkFX, fireworkFX_pos[k]);
            audiosource[1].PlayOneShot(SFXs[1]);

            yield return new WaitForSeconds(1);
        }
        k = 0;
    }

    public void startcoFinaleFX()
    {
        audiosource[0].PlayOneShot(SFXs[0]);
        StartCoroutine(finaleFX());
        StartCoroutine(fireworkkFX());
    }


    public void ScoreCheck_SHORT(int n)
    {
        if (gameNoteInstance_Rails[n][0] == null) return;

        dist = gameNoteInstance_Rails[n][0].transform.position.y - touchpads[n].transform.position.y;
        distAbs = Mathf.Abs(dist);


        if (distAbs > badZone)
        {
            //�������� ���̴ϱ� ��ġ�ص� �ǹ̰� ����.
            return;
            //passDestroy�� ���� �ʱ� ����
        }
        else if (distAbs > goodZone)
        {
            //Bad
            //showScoreText(3);
            ScoreManager.instance.StartShowScoreText("Bad",n,badScore);
            ScoreManager.instance.SCORE += badScore;
        }
        else if (distAbs > greatZone)
        {
            //Good
            //showScoreText(2);
            ScoreManager.instance.StartShowScoreText("Good",n,goodScore);
            ScoreManager.instance.SCORE += goodScore;
        }
        else if (distAbs > excellentZone)
        {
            //Great
            //showScoreText(1);
            ScoreManager.instance.StartShowScoreText("Great",n,greatScore);
            ScoreManager.instance.SCORE += greatScore;
        }
        else
        {
            //Excellent
            //showScoreText(0);
            ScoreManager.instance.StartShowScoreText("Excellent", n, excellentScore);
            ScoreManager.instance.SCORE += excellentScore;
        }

        //Handheld.Vibrate();
        PressDestroy(n);
    } //check_FINISHED!!!

    public void EnterCheck_LONG(int n)
    {
        print("*33333 EnterCheckLong�� ����Ǿ��� ���� noteInstance�� 0��° ĭ��" + gameNoteInstance_Rails[n][0] + "�� ����");
        if (gameNoteInstance_Rails[n][0] == null) return;

        dist = gameNoteInstance_Rails[n][0].transform.position.y - touchpads[n].transform.position.y;
        distAbs = Mathf.Abs(dist);


        if (distAbs < badZone)
        {
           
            //success
            print("*44444 LongNote�� Enter�� �����߰� ���� noteInstance�� 0��° ĭ�� isLongNotestart����" + gameNoteInstance_Rails[n][0].noteInfo.isLongNoteStart + "�� ����");
            //showScoreText(5);
            noteUnable(n);
            noteRemove(n);
            print("*55555 LongNote�� Enter�� �����߰� noteRemove���� �� ���� noteInstance�� 0��° ĭ�� isLongNotestart����" + gameNoteInstance_Rails[n][0].noteInfo.isLongNoteStart + "�� ����");
        }
        else
        {
            //���� �������� ��
            //��ġ�е� ���� �Ŀ� autoDestroy
            print("***** startNote�� ���� badZone���İ� ����");
            noteUnable(n);
            //�����ϸ� endNote�� üũ�ϵ��� ���������
            noteRemove(n);

        }
    } //check_FINISHED!!!

    public void ExitCheck_LONG(int n)
    {
        print("*66666 LongNote�� ���� ExitCheck�� ����Ǿ���.");

        dist = gameNoteInstance_Rails[n][0].transform.position.y - touchpads[n].transform.position.y;
        distAbs = Mathf.Abs(dist);

        if (distAbs < badZone)
        {
            //success
            //showScoreText(0);
            ScoreManager.instance.StartShowScoreText("Excellent", n, excellentScore);
            //noteUnable(n);
            //misscheck�� �ϸ� �ȵȴ�!!!

        }
        else if (dist > badZone)
        {
            //miss
            print("*77777 LongNote�� ���� Exit�� �����߰�, ���� longNote�� enable���´�" + gameNoteInstance_Rails[n][0].noteInfo.isNoteEnabled);
            noteUnable(n);
            print("*88888 LongNote�� ���� Exit�� �����߰�, unable�Լ� ���� ��, ���� longNote�� enable���´�" + gameNoteInstance_Rails[n][0].noteInfo.isNoteEnabled);

            //noteInstance_Rails[n].RemoveAt(0);

            MissCheck();
            print("*99999 longNoteExitCheck������ misscheck�� ����Ǿ���");
            //print("*22222-2 remove�Լ� ���� ��" + noteInstance_Rails[n][0] + "�� null �̾�� �ϴµ�!");
            //unabled
        }    
        
        noteRemove(n);
        //noteInstance_Rails[n].RemoveAt(0);
    }   //check_FINISHED!!!

    public void EnterCheck_DRAG(int n)
    {
        if (gameNoteInstance_Rails[n][0] == null) return;

        dist = gameNoteInstance_Rails[n][0].transform.position.y - touchpads[n].transform.position.y;
        distAbs = Mathf.Abs(dist);


        if(distAbs < badZone)
        {
            //success
            print("***dragEnter ���� �ߴ�!");
        }
        //else if (dist < 0)
        //{
        //    //Ȥ�ó� startIndex������ �ʰ� autoDestroy�� �ɸ��� Ÿ�ֿ̹� unable ���� üũ ���ص� �ɰ� ���� ��.
        //    noteUnable(n);
        //    print("***noteUnable �Ǿ���!");
        //    return;
        //}


    }   //check FINISHED!!!

    public void ExitCheck_DRAG(int n)
    {
        if (gameNoteInstance_Rails[n][0] == null) return;

        //�̹� �� ���� �ùٸ� ��ġ��� ���� Ȯ���� �Ĵϱ�!!!
        distAbs = Mathf.Abs(touchpads[n].transform.position.y - gameNoteInstance_Rails[n][0].transform.position.y);
        dist = gameNoteInstance_Rails[n][0].transform.position.y - touchpads[n].transform.position.y;
        
        if (distAbs < badZone)
        {
            //success
            if (touchReleasedIdx == gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.DRAG_release_idx && draggingState == DraggingState.Dragging_LEFT && gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.type == (int)GameNoteType.DRAG_LEFT)
            {
                //success

                print("���� �巡�� ��Ʈ �����߾��!");
                PressDestroy(touchStartedIdx);
                //showScoreText(0);
                ScoreManager.instance.StartShowScoreText("Excellent",n,excellentScore);
            }
            else if (touchReleasedIdx == gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.DRAG_release_idx && draggingState == DraggingState.Dragging_RIGHT && gameNoteInstance_Rails[touchStartedIdx][0].noteInfo.type == (int)GameNoteType.DRAG_RIGHT)
            {
                print("������ �巡�� ��Ʈ �����߾��!");
                PressDestroy(touchStartedIdx);
                //showScoreText(0);
                ScoreManager.instance.StartShowScoreText("Excellent", n, excellentScore);
            }
            else
            {
                MissCheck();
            }

        }
        else
        {
            //miss
            MissCheck();
        }
    }


    public void PressingScore(int n)
    {
        if (gameNoteInstance_Rails[n][0] == null) return;

        distAbs = Mathf.Abs(touchpads[n].transform.position.y - gameNoteInstance_Rails[n][0].transform.position.y);
        dist = gameNoteInstance_Rails[n][0].transform.position.y - touchpads[n].transform.position.y;

        //if (distAbs < badZone)
        {
            ScoreManager.instance.SCORE += pressScore * Time.deltaTime ;

        }
        //else
        {
            //note�� ���� ���� ���� ���� ���� ��� score���� X
        }
    }

    public void MissCheck()
    {
        //showScoreText(4);
        ScoreManager.instance.StartShowScoreText("Miss",0,0);

        //�ȵǴ� ����..? ���������̶�?
        CamShake.instance.StartShake(0.2f, 0.5f, 1);      
    }

    public void PressDestroy(int n)
    {
        Destroy(gameNoteInstance_Rails[n][0].gameObject);
        gameNoteInstance_Rails[n].RemoveAt(0);

        //note���� FX������
    }

    public void MissUnabled(int n)
    {
        print("note�� enabled == false�Ǿ���");
        //long�̳� drag�� �����ٰ� ������ ������ ���� ���
        //passDestroy������ �Ⱓ ���� ���� üũ�� ���� ���ϵ��� �ؾ���.

        gameNoteInstance_Rails[n][0].noteInfo.isNoteEnabled = false;
        gameNoteInstance_Rails[n].RemoveAt(0);
    }

    //01. NoteType.SHORT test
    #region SHORT
    void InputTestSHORTNotes()
    {
        NoteData info = new NoteData();

        info.railIdx = 0;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1 * bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 2 * bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 3 * bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 4 * bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 5 * bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 5;
        info.type = (int)GameNoteType.SHORT;
        info.time = 6 * bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 7 * bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 8 * bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);


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

    //02. NoteType.Long test
    #region LONG
    void InputTestLONGNotes()
    {
        NoteData info = new NoteData();

        info.railIdx = 1;
        info.type = (int)GameNoteType.LONG;
        info.time = 1 *bpm;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info.railIdx = 1;
        info.type = (int)GameNoteType.LONG;
        info.time = 4 * bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);


        //info.railIdx = 3;
        //info.type = (int)NoteType.LONG;
        //info.time = 4;
        //info.isLongNoteStart = true;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info.railIdx = 3;
        //info.type = (int)NoteType.LONG;
        //info.time = 5;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

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

    //03. NoteType.Drag test
    #region DRAG
    void InputTestDRAGNote()
    {
        NoteData info = new NoteData();

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.DRAG_RIGHT;
        info.time = 1*bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 5;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.DRAG_LEFT;
        info.time = 4 * bpm;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 4 * bpm;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 4 * bpm;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 3;
        //info.type = (int)NoteType.DRAG_RIGHT;
        //info.time = 6 * bpm;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 5;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

        //info = new NoteInfo();
        //info.railIdx = 2;
        //info.type = (int)NoteType.DRAG_LEFT;
        //info.time = 6 * bpm;
        //info.isLongNoteStart = false;
        //info.DRAG_release_idx = 0;
        //info.isNoteEnabled = true;
        //allNoteInfo.Add(info);

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

    //04. Mixed test
    #region MIXED
    void InputTestMIXEDNote()
    {
        NoteData info = new NoteData();

        info.railIdx = 0;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1;
        info.isLongNoteStart = false;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 3;
        info.isLongNoteStart = false;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 5;
        info.type = (int)GameNoteType.SHORT;
        info.time = 4;
        info.isLongNoteStart = false;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 6;
        info.isLongNoteStart = false;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 6;
        info.isLongNoteStart = false;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 6;
        info.isLongNoteStart = false;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 7;
        info.isLongNoteStart = false;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 5;
        info.type = (int)GameNoteType.SHORT;
        info.time = 9;
        info.isLongNoteStart = false;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.LONG;
        info.time = 2;
        info.isLongNoteStart = true;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.LONG;
        info.time = 4;
        info.isLongNoteStart = false;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.LONG;
        info.time = 7;
        info.isLongNoteStart = true;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.LONG;
        info.time = 8;
        info.isLongNoteStart = false;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.DRAG_RIGHT;
        info.time = 5;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 5;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.DRAG_LEFT;
        info.time = 5;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

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

    //05. FLOP test
    #region FLOP
    void InputTestFLOP()
    {
        NoteData info = new NoteData();

        #region Pattern01

        //���� 1) Pattern 1 - Short
        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 31;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 61;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 76;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 5;
        info.type = (int)GameNoteType.SHORT;
        info.time = 121;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 151;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 181;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 211;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        //���� 2) Pattern 1
        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 241;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 271;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 301;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 316;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 5;
        info.type = (int)GameNoteType.SHORT;
        info.time = 361;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 391;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 421;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 451;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        //���� 3) Pattern 1
        info = new NoteData();
        info.railIdx = 0;
        info.type = (int)GameNoteType.SHORT;
        info.time = 481;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 511;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 541;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 556;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 601;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 631;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 661;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 691;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        //���� 4) Pattern 1
        info = new NoteData();
        info.railIdx = 0;
        info.type = (int)GameNoteType.SHORT;
        info.time = 721;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 751;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 781;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 796;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 841;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 871;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 901;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.SHORT;
        info.time = 931;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);


        #endregion

        #region Pattern02
        //���� 5) Pattern 2
        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 961;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 976;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 1006;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1021;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.LONG;
        info.time = 1043.5f;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.LONG;
        info.time = 1058.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1081f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1096f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 1126f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.LONG;
        info.time = 1141f;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.LONG;
        info.time = 1178.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        //���� 6) Pattern 2
        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1201;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1216;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 1246;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1261;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 1327.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1342.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.LONG;
        info.time = 1365;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 5;
        info.type = (int)GameNoteType.LONG;
        info.time = 1411;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 1463.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1478.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 1508.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.LONG;
        info.time = 1523.5f;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        //***
        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.LONG;
        info.time = 1550;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 1583.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1598.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.LONG;
        info.time = 1621;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.LONG;
        info.time = 1658.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        //���� 8) Pattern 2

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1681;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1696;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 1726;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1741;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 1807.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 1822.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.LONG;
        info.time = 1845;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.LONG;
        info.time = 1891;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 2011;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.DRAG_RIGHT;
        info.time = 2101;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 5;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 2191;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.DRAG_RIGHT;
        info.time = 2281;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 5;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 2371;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 2;
        info.type = (int)GameNoteType.SHORT;
        info.time = 2393.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 2431;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.DRAG_RIGHT;
        info.time = 2521;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 5;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 2611;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.DRAG_LEFT;
        info.time = 2701;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = 0;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 2791;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 2821;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 2873.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);
        #endregion

        #region Pattern04

        //���� 13-14) Pattern 4
        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.LONG;
        info.time = 2881;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.LONG;
        info.time = 3090;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.LONG;
        info.time = 3091;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.LONG;
        info.time = 3300;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = false;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.SHORT;
        info.time = 3301;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 3323.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.LONG;
        info.time = 3361;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 4;
        info.type = (int)GameNoteType.LONG;
        info.time = 3570;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.LONG;
        info.time = 3571;
        info.isLongNoteStart = true;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 1;
        info.type = (int)GameNoteType.LONG;
        info.time = 3780;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
        info.type = (int)GameNoteType.SHORT;
        info.time = 3811;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
        allGameNoteInfo.Add(info);

        info = new NoteData();
        info.railIdx = 3;
        info.type = (int)GameNoteType.SHORT;
        info.time = 3833.5f;
        info.isLongNoteStart = false;
        info.DRAG_release_idx = -1;
        info.isNoteEnabled = true;
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
    void showScoreText(int n)
    {
        GameObject scoreText = Instantiate(scoreTexts[n], canvas.transform.position - Vector3.forward*2, Quaternion.identity);
        scoreText.transform.SetParent(canvas.transform);

        Destroy(scoreText, 0.5f);
    }

}
