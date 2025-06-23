using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class EJScoreManager : MonoBehaviour
{
    public static EJScoreManager instance;

    public TextMeshProUGUI textScore;
    public TextMeshProUGUI numScore;
    public TextMeshProUGUI[] score4rails;

    public Slider scoreSlider;
    public int maxScore;

    float score;

    public Canvas canvas;
    //public GameObject[] scoreTexts;

    private void Awake()
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        SCORE = 0;

        scoreSlider.maxValue = 200;
    }

    // Update is called once per frame
    void Update()
    {
                   
    }

    public float SCORE
    {
        get
        {
            return score;
        }
        set
        {
            
            score = value;
            
            numScore.text = "Score : " + score;
            scoreSlider.value = score;
        }
    }

    public void SetSCORE(int value)
    {
        score = value;
    }

    public float GetSCORE()
    {
        return score;
    }

    public IEnumerator showScoreText(string sss, int railIdx, int score)
    {

        //ó��
        //GameObject scoreText = Instantiate(scoreTexts[n], canvas.transform.position - Vector3.forward, Quaternion.identity);
        //scoreText.transform.SetParent(canvas.transform);

        //Destroy(scoreText, 0.5f);
        textScore.text = sss + "!";

        if (!(sss == "Miss"))
        {
            score4rails[railIdx].text = "+" + score;
        }

        yield return new WaitForSeconds(0.8f);

        score4rails[railIdx].text = "";
        textScore.text = "";

    }

    public void StartShowScoreText(string sss, int railIdx, int score)
    {
        StartCoroutine(showScoreText(sss, railIdx, score));
    }


}
