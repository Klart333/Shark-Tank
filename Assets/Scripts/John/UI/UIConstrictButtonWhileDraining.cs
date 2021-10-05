using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConstrictButtonWhileDraining : MonoBehaviour
{
    private Button button;
    private UIDrainExp drainExp;

    private void Start()
    {
        button = GetComponent<Button>();
        drainExp = FindObjectOfType<UIDrainExp>();
    }

    private void Update()
    {
        if (drainExp != null)
        {
            if (drainExp.Draining)
            {
                button.interactable = false;
                return;
            }
        }

        if (!button.interactable)
        {
            button.interactable = true;
        }
    }
}
