using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージなどの際にカメラを揺らすスクリプト
/// </summary>
public class CamShake : MonoBehaviour
{
    public static CamShake instance;

    float shakeDuration = 0.2f;
    float shakeAmount = 0.5f;
    float dampingSpeed = 0.9f;

    Vector3 originalPos;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        // 揺れ終了後に戻す位置を記録（OnEnable : 再有効化時にも対応）
        originalPos = transform.localPosition;

        NoteJudge.OnNoteJudged += HandleCamShake;
    }

    void OnDisable()
    {
        NoteJudge.OnNoteJudged -= HandleCamShake;
    }

    void HandleCamShake(NoteJudgedEventData data)
    {
        if(data.result == JudgeResult.Miss)
        StartShake(shakeDuration, shakeAmount, dampingSpeed);
    }

    // duration中、毎フレームランダムに位置を変化
    public IEnumerator ShakeCamera(float duration, float amount, float damping)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * amount;

            elapsed += Time.deltaTime * damping;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void StartShake(float duration, float amount, float damping)
    {
        StartCoroutine(ShakeCamera(duration, amount, damping));
    }
}
