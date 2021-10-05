using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayActiveBackground : MonoBehaviour
{
    public Sprite[] Backgrounds;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        Sprite backgroundToSet = Backgrounds[0];

        for (int i = 0; i < Backgrounds.Length; i++)
        {
            if (Backgrounds[i].name == PlayerPrefs.GetString("ActiveBackground"))
            {
                backgroundToSet = Backgrounds[i];
            }
        }

        spriteRenderer.sprite = backgroundToSet;
    }

}
