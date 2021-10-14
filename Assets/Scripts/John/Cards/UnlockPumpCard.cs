using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPumpCard : CardOption
{
    public override void SelectCard()
    {
        base.SelectCard();

        PlayerPrefs.SetInt("UnlockedPump", 1);
    }
}
