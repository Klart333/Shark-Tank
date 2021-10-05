using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetActiveThing : MonoBehaviour
{
    public string Thing = "ActiveShark";

    public string[] ThingNames = new string[7] {"OGShark", "CyanShark", "RedShark", "GreenShark", "WhiteShark", "GoldShark", "AntiShark" };

    public RectTransform[] ThingPanels = new RectTransform[7];
    public Image[] Locks = new Image[7];

    public int LevelIntervalToUnlockTheThing___Yes;

    private bool[] unlocked;

    // Order is very important
    private int thingIndex = 0;
    private float thingWidth = 0;

    private int previousIndex = -1;

    private float speed = 2;
    private bool changing = false;

    private void Start()
    {
        unlocked = new bool[ThingNames.Length];

        thingWidth = ThingPanels[0].rect.size.x;

        for (int i = 0; i < ThingPanels.Length; i++)
        {
            ThingPanels[i].position += new Vector3(thingWidth * i, 0, 0);
        }

        LevelInfo levelInfo = Level.GetLevelInfo();

        for (int i = 0; i < Locks.Length; i++)
        {
            if (levelInfo.Level >= i * LevelIntervalToUnlockTheThing___Yes)
            {
                Locks[i].enabled = false;
                unlocked[i] = true;
            }
        }
    }

    public void GoLeft()
    {
        StartCoroutine(GoingLeft());
    }

    public IEnumerator GoingLeft()
    {
        if (thingIndex > 0 && !changing)
        {
            thingIndex--;
            changing = true;

            float t = 0;

            Vector3[] originalPositions = new Vector3[7];
            for (int i = 0; i < ThingPanels.Length; i++)
            {
                originalPositions[i] = ThingPanels[i].position;
            }

            while (t <= 1)
            {
                for (int i = 0; i < ThingPanels.Length; i++)
                {
                    ThingPanels[i].position = originalPositions[i] + new Vector3(thingWidth * t, 0, 0);
                }

                t += Time.deltaTime * speed;

                yield return null;
            }

            for (int i = 0; i < ThingPanels.Length; i++)
            {
                ThingPanels[i].position = originalPositions[i] + new Vector3(thingWidth, 0, 0);
            }

            changing = false;
        }
    }

    public void GoRight()
    {
        StartCoroutine(GoingRight());
    }

    public IEnumerator GoingRight()
    {
        if (thingIndex < ThingNames.Length - 1 && !changing)
        {
            thingIndex++;
            changing = true;

            float t = 0;

            Vector3[] originalPositions = new Vector3[7];
            for (int i = 0; i < ThingPanels.Length; i++)
            {
                originalPositions[i] = ThingPanels[i].position;
            }

            while (t <= 1)
            {
                for (int i = 0; i < ThingPanels.Length; i++)
                {
                    ThingPanels[i].position = originalPositions[i] - new Vector3(thingWidth * t, 0, 0);
                }

                t += Time.deltaTime * speed;

                yield return null;
            }
            for (int i = 0; i < ThingPanels.Length; i++)
            {
                ThingPanels[i].position = originalPositions[i] - new Vector3(thingWidth, 0, 0);
            }


            changing = false;
        }
    }

    public void SelectThing()
    {
        if (!changing && unlocked[thingIndex])
        {
            PlayerPrefs.SetString(Thing, ThingNames[thingIndex]);

            if (previousIndex != -1)
            {
                ThingPanels[previousIndex].GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }

            previousIndex = thingIndex;

            ThingPanels[thingIndex].GetComponent<Image>().color = new Color(0, 255, 0, 1);
        }
    }
}
