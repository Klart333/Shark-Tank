using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DrunkCard : CardOption
{
    private bool lastFrameActive = false;

    private float speed = 1;

    private void Update()
    {
        if (IsActive && lastFrameActive != IsActive)
        {
            StartCoroutine(GetDrunk());
            StartCoroutine(JitterCamera());
        }

        lastFrameActive = IsActive;
    }

    private IEnumerator JitterCamera()
    {
        var cam = FindObjectOfType<Camera>();
        Vector3 originalPos = cam.transform.position;
        while (!GameManager.Instance.Frozen)
        {
            yield return new WaitForSeconds(Time.deltaTime * 3);

            cam.transform.position = originalPos + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0);
        }
    }

    private IEnumerator GetDrunk()
    {
        yield return null;
        Volume volume = FindObjectOfType<Volume>();
        volume.profile.TryGet(out ChromaticAberration chrome);

        float t = 0;

        while (t <= 1)
        {
            t += Time.deltaTime * speed;

            chrome.intensity.value = t;

            yield return null;
        }

        chrome.intensity.value = 1.0f;
    }
}
