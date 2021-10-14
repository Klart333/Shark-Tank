using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBackgroundType : MonoBehaviour
{
    public const string BackgroundPlayerPref = "Background";

    private string[] BackgroundNames = new string[] { "Default", "Dawn", "UpsideDown", "BloodMoon" };

    [SerializeField]
    private int backgroundUnlockInterval = 20;

    [SerializeField]
    private Transform backgroundPanelsParent;

    private Transform[] backgroundPanels;
    private GameObject[] backgroundLocks;
    private GameObject[] backgroundMarkers;

    private bool[] unlocked;

    private int currentSelectedIndex = -1;

    private void Start()
    {
        backgroundPanels = new Transform[backgroundPanelsParent.childCount];
        backgroundLocks = new GameObject[backgroundPanels.Length];
        backgroundMarkers = new GameObject[backgroundPanels.Length];

        for (int i = 0; i < backgroundPanelsParent.childCount; i++)
        {
            backgroundPanels[i] = backgroundPanelsParent.GetChild(i);
            backgroundLocks[i] = backgroundPanels[i].GetChild(1).gameObject;
            backgroundMarkers[i] = backgroundPanels[i].GetChild(2).gameObject;
        }

        unlocked = new bool[BackgroundNames.Length];

        LevelInfo levelInfo = Level.GetLevelInfo();

        for (int i = 0; i < backgroundLocks.Length; i++)
        {
            if (levelInfo.Level >= i * backgroundUnlockInterval)
            {
                backgroundLocks[i].SetActive(false);
                unlocked[i] = true;
            }
        }

        if (string.IsNullOrEmpty(PlayerPrefs.GetString(BackgroundPlayerPref)))
        {
            currentSelectedIndex = 0;
            PlayerPrefs.SetString(BackgroundPlayerPref, BackgroundNames[0]);

            backgroundMarkers[0].SetActive(true);
        }
        else
        {
            for (int i = 0; i < BackgroundNames.Length; i++)
            {
                if (BackgroundNames[i] == PlayerPrefs.GetString(BackgroundPlayerPref))
                {
                    backgroundMarkers[i].SetActive(true);
                    currentSelectedIndex = i;

                    break;
                }
            }
        }
    }

    public void SelectBackground(int index)
    {
        if (unlocked[index])
        {
            PlayerPrefs.SetString(BackgroundPlayerPref, BackgroundNames[index]);

            backgroundMarkers[currentSelectedIndex].SetActive(false);
            backgroundMarkers[index].SetActive(true);
            currentSelectedIndex = index;
        }
    }
}
