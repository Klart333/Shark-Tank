using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIResponseText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI responseText;

    bool increasingSize = false; // For effect we want the increasing to keep increasing

    public IEnumerator Print(string text, float time)
    {
        responseText.text = text;

        object[] parms = new object[2] { 0.5f, text };

        StartCoroutine("GrowTextThenShrink", parms);
        yield return new WaitForSeconds(time / 2);
        StartCoroutine("ShrinkText", parms);
        yield return new WaitForSeconds(time / 2);

        ResetText();
    }

    public void PrintDef(string text) // Default Time
    {
        responseText.text = text;

        StartCoroutine("GrowTextThenShrink", 0.5f);
    }

    private IEnumerator GrowTextThenShrink(float timeToGrow) // Grows the text 
    {
        float maxFontIncrease = 50;
        for (int i = 0; i < maxFontIncrease; i++) // Reason for some things being repeatedly set in the loop is because it would be set otherwise in the ShrinkText and ResetText
        {
            increasingSize = true;

            if (responseText.fontSize < 140)
            {
                responseText.fontSize = responseText.fontSize + 1;
            }
            yield return new WaitForSeconds(timeToGrow / maxFontIncrease);
        }
        increasingSize = false;
        StartCoroutine("ShrinkText", timeToGrow);
    }

    private IEnumerator ShrinkText(float timeToShrink)
    {
        float maxFontIncrease = 50;
        for (int i = 0; i < maxFontIncrease; i++)
        {
            if (increasingSize == false) // If we are still increasing the size of the text we don't want it suddenly starting to shrink and bug from multiple calls
            {
                if (responseText.fontSize > 80)
                {
                    responseText.fontSize -= 2;
                }

                yield return new WaitForSeconds(timeToShrink / maxFontIncrease);
            }

        }
        if (increasingSize == false)
        {
            ResetText(); // On the last call we can reset
        }
    }


    private void ResetText()
    {
        responseText.fontSize = 80; 
        responseText.text = "";
    }

}
