using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSharkType : MonoBehaviour
{
    public const string SharkPlayerPref = "ActiveShark";

    private string[] SharkNames = new string[] { "OGShark", "CyanShark", "RedShark", "GreenShark", "WhiteShark", "GoldShark", "AntiShark", "PumpShark" };

    [SerializeField]
    private int sharkUnlockInterval = 10;

    [SerializeField]
    private Transform sharkPanelsParent;

    private Transform[] sharkPanels;
    private GameObject[] sharkLocks;
    private GameObject[] sharkMarkers;

    private bool[] unlocked;

    private int currentSelectedIndex = -1;

    private void Start()
    {
        sharkPanels = new Transform[sharkPanelsParent.childCount];
        sharkLocks = new GameObject[sharkPanels.Length];
        sharkMarkers = new GameObject[sharkPanels.Length];

        for (int i = 0; i < sharkPanelsParent.childCount; i++)
        {
            sharkPanels[i] = sharkPanelsParent.GetChild(i);
            sharkLocks[i] = sharkPanels[i].GetChild(1).gameObject;
            sharkMarkers[i] = sharkPanels[i].GetChild(2).gameObject;
        }

        unlocked = new bool[SharkNames.Length];

        LevelInfo levelInfo = Level.GetLevelInfo();

        for (int i = 0; i < sharkLocks.Length; i++)
        {
            if (SharkNames[i] == "PumpShark")
            {
                if (PlayerPrefs.GetInt("UnlockedPump") == 1)
                {
                    sharkLocks[i].SetActive(false);
                    unlocked[i] = true;
                }

                continue;
            }

            if (levelInfo.Level >= i * sharkUnlockInterval)
            {
                sharkLocks[i].SetActive(false);
                unlocked[i] = true;
            }
        }

        if (string.IsNullOrEmpty(PlayerPrefs.GetString(SharkPlayerPref)))
        {
            currentSelectedIndex = 0;
            PlayerPrefs.SetString(SharkPlayerPref, SharkNames[0]);

            sharkMarkers[0].SetActive(true);
        }
        else
        {
            for (int i = 0; i < SharkNames.Length; i++)
            {
                if (SharkNames[i] == PlayerPrefs.GetString(SharkPlayerPref))
                {
                    sharkMarkers[i].SetActive(true);
                    currentSelectedIndex = i;

                    break;
                }
            }
        }
    }

    public void SelectShark(int index)
    {
        if (unlocked[index])
        {
            PlayerPrefs.SetString(SharkPlayerPref, SharkNames[index]);

            sharkMarkers[currentSelectedIndex].SetActive(false);
            sharkMarkers[index].SetActive(true);
            currentSelectedIndex = index;
        }
    }
}
