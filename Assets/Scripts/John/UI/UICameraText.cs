using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UICameraText : MonoBehaviour // Just displayes the amount of flashes the player has left
{
    [SerializeField]
    private CameraFlash cameraFlash;

    TextMeshProUGUI text;

    private void Start()
    {
        cameraFlash.OnFlash += UpdateText;

        text = GetComponent<TextMeshProUGUI>();
    }

    private void UpdateText()
    {
        text.text = "Camera Flash \n" + cameraFlash.cameraFlashes.ToString() + "x";
    }
}
