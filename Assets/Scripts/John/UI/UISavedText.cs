using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UISavedText : MonoBehaviour
{
    public float speed = 1;

    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float t = 0;

        while (t <= 1)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - t);

            t += Time.deltaTime * speed;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
