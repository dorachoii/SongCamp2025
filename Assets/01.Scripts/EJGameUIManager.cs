using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJGameUIManager : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI artist;

    public GameObject UIcanvas;
    public GameObject successText;

    public static EJGameUIManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void successUI()
    {
        StartCoroutine(gameEndUI());
    }

    public IEnumerator gameEndUI()
    {
        UIcanvas.SetActive(false);
        successText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        successText.SetActive(false);
        UIcanvas.SetActive(true);
        yield return null;
    }
}
