using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    [SerializeField]
    private CardOption[] allCards;

    [SerializeField]
    private Transform cardParent;

    [SerializeField]
    private GameObject adPanel;

    private List<CardOption> cardOptions = new List<CardOption>();

    private AdsManager adsManager;

    private bool shuffled = false;

    private void Start()
    {
        StartCoroutine(GenerateCards());

        adsManager = FindObjectOfType<AdsManager>();
        adsManager.OnAdFinished += CardHandler_OnAdFinished;
    }

    private void CardHandler_OnAdFinished(bool watched)
    {
        if (watched && !shuffled)
        {
            shuffled = true;

            float tim = 0.3f;
            for (int i = 0; i < cardParent.childCount; i++)
            {
                Destroy(cardParent.GetChild(i).gameObject, tim -= (tim/3.0f));
            }

            StartCoroutine(GenerateCards());
        }
    }

    private IEnumerator GenerateCards()
    {
        PickRandomCards();
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < cardOptions.Count; i++)
        {
            Instantiate(cardOptions[i], cardParent);
            yield return new WaitForSeconds(1f);
        }

        if (!shuffled)
        {
            adPanel.SetActive(true);
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

    public void SelectedCard(CardOption card)
    {
        card.IsActive = true;
        GetComponent<CanvasGroup>().interactable = false;
        adsManager.OnAdFinished -= CardHandler_OnAdFinished;

        StartCoroutine(ClosePanelAndStartGame());
    }

    private IEnumerator ClosePanelAndStartGame()
    {
        yield return new WaitForSeconds(1f);

        GetComponent<Animator>().SetTrigger("ClosePanel");

        yield return new WaitForSeconds(2f);

        GameManager.Instance.StartGame();
    }
}
