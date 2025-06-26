using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownController : MonoBehaviour
{
    public TMP_Text countdownText;
    public event Action OnCountdownComplete;

    public void StartCountdown()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        for (int i = 5; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "START!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        OnCountdownComplete?.Invoke();  // ✅ 이벤트 발생
    }
}

