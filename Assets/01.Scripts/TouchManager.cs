using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public GameObject[] rails;
    public Material[] activeMats;
    public Material[] defaultMats;
    private NoteJudge noteJudge;

    // Start is called before the first frame update
    void Start()
    {
        noteJudge = FindObjectOfType<NoteJudge>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: 터치 보정
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 100f))
            {
                GameObject hitObj = hitInfo.collider.gameObject;
                string name = hitObj.name;

                if (name.StartsWith("Touch_Box"))
                {
                    string numStr = name.Substring("Touch_Box".Length);
                    if (int.TryParse(numStr, out int index))
                    {
                        index -= 1;
                        rails[index].GetComponent<MeshRenderer>().material = activeMats[index];
                        noteJudge.JudgeShortNote(index);
                    }
                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < rails.Length; i++)
            {
                rails[i].GetComponent<MeshRenderer>().material = defaultMats[i];
            }
        }
    }
}
