using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlashCamera : MonoBehaviour // Sets the alpha of a panel in the canvas, may be a little scuffed method of a screen flash, but it works fine so whatever
{
    private CameraFlash cameraFlash;
    private CanvasRenderer canvasRenderer;

    private float speed = 5f;

    private void Start()
    {
        canvasRenderer = GetComponent<CanvasRenderer>();

        cameraFlash = FindObjectOfType<CameraFlash>();
        cameraFlash.OnFlash += Flash;
    }

    private void Flash()
    {
        StartCoroutine(FlashThePanel());
    }

    private IEnumerator FlashThePanel()
    {
        float t = 0;

        while (t <= 1)
        {
            t += Time.deltaTime * speed;

            canvasRenderer.SetAlpha(t);

            yield return null;
        }

        while (t >= 0)
        {
            t -= Time.deltaTime * speed;

            canvasRenderer.SetAlpha(t);

            yield return null;
        }

        canvasRenderer.SetAlpha(0);
    }
}
