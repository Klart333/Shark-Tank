using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIDropShadowText : MonoBehaviour
{
    public TextMeshProUGUI ParentText;
    
    private TextMeshProUGUI dropShadowText;

    private void Start()
    {
        dropShadowText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (dropShadowText.text != ParentText.text)
        {
            dropShadowText.text = ParentText.text;
        }
    }
}
