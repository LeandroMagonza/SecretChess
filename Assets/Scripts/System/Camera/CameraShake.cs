using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public delegate void CameraShakeDelegate(float duration, float magnitude);
    public static CameraShakeDelegate Shake;

    private void OnEnable()
    {
        Shake += StartShake;
    }
    private void OnDisable()
    {
        Shake -= StartShake;
    }
    private void StartShake(float duration, float magnitude)
    {
        if (Options.shake)
            StartCoroutine(CamShake(duration, magnitude));
    }
    public IEnumerator CamShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = originalPos.x + Random.Range(-1,1) * magnitude;
            float y = originalPos.y + Random.Range(-1,1) * magnitude;
            transform.localPosition = new Vector3(x,y,originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
