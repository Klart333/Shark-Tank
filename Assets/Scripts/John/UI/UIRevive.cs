using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIRevive : MonoBehaviour
{
    public GameObject content;
    public TextMeshProUGUI timerText;

    private bool activated = false;

    private float timer = 6;

    public void ToggleRevive(bool active)
    {
        activated = active;
        content.SetActive(active);
    }

    private void Update()
    {
        if (activated)
        {
            timer -= Time.deltaTime;

            timerText.text = Mathf.FloorToInt(timer).ToString();

            if (timer <= 0.01f)
            {
                WatchedAd(false);
            }
        }
    }

    public void WatchingAd()
    {
        timer = 0;
        activated = false;
    }

    public void WatchedAd(bool watched)
    {
        ToggleRevive(false);

        if (!watched)
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.SwitchSceneAfterDelay(0.5f));
        }
        else
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.ContinueGame());
            GameManager.Instance.Frozen = false;
        }

    }
}
