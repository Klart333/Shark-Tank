using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOption : MonoBehaviour
{
    public bool IsActive = false;

    public virtual void SelectCard()
    {
        AudioManager.Instance.PlayClickSound();

        FindObjectOfType<CardHandler>().SelectedCard(this);
        GetComponent<Animator>().SetTrigger("Selected");
    }
}
