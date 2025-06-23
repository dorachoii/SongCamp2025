using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class iTweenTest : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TextUp(text);
        }
    }

    //��� �����;� �ϴ� ��
    //01.

    void TextUp(TextMeshProUGUI text)
    {
        iTween.MoveTo(text.gameObject, iTween.Hash(
                                            "position", text.rectTransform.position += new Vector3(-2, 0, 0),
                                            "time", 0.5f,
                                            "easetype", iTween.EaseType.easeInOutExpo,
                                            "oncomplete", "OnMoveComplete",
                                            "oncompletetarget", gameObject
                                            ));

        iTween.ColorTo(text.gameObject, iTween.Hash(
                                            "a", 0f,  // ������ 0.5�� ���� (0���� 1������ ��)
                                            "time", 0.5f,
                                            "easetype", iTween.EaseType.easeInOutExpo
                                            ));

        iTween.ScaleTo(gameObject, iTween.Hash(
                                            "scale", new Vector3(2, 2, 2),  // x, y, z ���� ũ�⸦ 2�� ����
                                            "time", 2.0f,
                                            "easetype", iTween.EaseType.easeInOutExpo
                                            ));

        iTween.FadeTo(gameObject, iTween.Hash(
                                            "alpha", 0.5f,  // ������ 0.5�� ���� (0���� 1������ ��)
                                            "time", 2.0f,
                                            "easetype", iTween.EaseType.easeInOutExpo
                                            ));

    }
}
