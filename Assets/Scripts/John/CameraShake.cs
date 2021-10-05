using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator ScreenShakeSinWave(Vector2[] parms)
    {
        // Parameter One = Shake Amplitude
        // Parameter Two = Shake frequency
        // Parameter Three = The amount of shakes (only x is used, still a vector2 for simplicity)

        Vector2 shakeAmplitude = parms[0];
        Vector2 shakeFrequency = parms[1];
        Vector2 shakeAmountOfTimes = parms[2];

        Vector2 shakeTime = Vector2.zero;
        Vector3 position = transform.localPosition;

        for (int i = 0; i < shakeAmountOfTimes.x; i++)
        {
            shakeTime += new Vector2(Time.deltaTime, Time.deltaTime) * shakeFrequency;
            position = new Vector3(Mathf.Sin(shakeTime.x), Mathf.Sin(shakeTime.y), 0) * shakeAmplitude;
            transform.localPosition = position;

            yield return new WaitForSeconds(0.001f);
        }


        transform.localPosition = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
    }
}
