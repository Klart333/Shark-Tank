using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    [SerializeField]
    private CardOption[] allCards;

    private List<CardOption> cardOptions = new List<CardOption>();

    private void Start()
    {
        StartCoroutine(GenerateCards());
    }

    private IEnumerator GenerateCards()
    {
        PickRandomCards();

        for (int i = 0; i < cardOptions.Count; i++)
        {
            Instantiate(cardOptions[i], transform);
            yield return new WaitForSeconds(2f);
        }
    }

    private void PickRandomCards()
    {
        cardOptions.Clear();
        List<int> takenIndexes = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int index = 0;
            do
            {
                index = UnityEngine.Random.Range(0, allCards.Length);
            } while (takenIndexes.Contains(index));

            cardOptions.Add(allCards[index]);
            takenIndexes.Add(index);
        }
    }
}
