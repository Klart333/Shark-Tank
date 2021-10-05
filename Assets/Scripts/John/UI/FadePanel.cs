using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
    private Image fadeImage;

    private float alpha;
    private float fadeSpeed = 2f;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeIn()
    {
        alpha = 1;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;

            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }
    }

    public IEnumerator FadeOut(UILoadScene loadScene)
    {
        alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;

            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }

        loadScene.ChangeScene();
    }

}
