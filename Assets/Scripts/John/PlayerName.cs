using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerName : MonoBehaviour
{
    private TMP_InputField nameField;
    private void Start()
    {
        nameField = GetComponent<TMP_InputField>();

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))
        {
            nameField.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void SetPlayerName()
    {
        PlayerPrefs.SetString("PlayerName", nameField.text);
    }
}
