using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIDrainExp : MonoBehaviour
{
    public bool Draining = true;

    [SerializeField]
    private TextMeshProUGUI expText;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private Image levelFlash;

    private CanvasGroup canvasRenderer;

    private LevelInfo levelInfo;

    private bool stupidDraining = false;
    private float fillSpeed = 1f;
    private float fadeSpeed = 0.6f;
    private float timeFirstSlide = 0;
    private float timePerSlide = 0;

    private void Start()
    {
        canvasRenderer = GetComponent<CanvasGroup>();
    }

    private void Update()
    { 
        if (!stupidDraining)
        {
            int n = 0;
            int.TryParse(FindObjectOfType<ScorePanele>().GetComponent<TextMeshProUGUI>().text, out n);
            StartCoroutine(StartDrain(n));
        }
    }

    public IEnumerator StartDrain(int expAmount)
    {
        levelInfo = Level.GetLevelInfo();

        stupidDraining = true;
        Draining = true;

        expText.text = string.Format("Exp: {0}", expAmount);
        levelText.text = levelInfo.Level.ToString();
        levelFlash.fillAmount = (float)levelInfo.Experience / (float)Level.ExpToNextLevel;

        float totalTime = Mathf.Pow(expAmount / 50, 0.2f) + 3;
        int slides = Level.AmountOfLevels(expAmount) + 1;

        if (slides > 1)
        {
            float firstPercent = (float)Level.CurrentExperience / (float)Level.ExpToNextLevel;
            timeFirstSlide = (totalTime / slides) * (1.0f - firstPercent);

            timePerSlide = (totalTime / (slides - 1)) + ((totalTime / slides) * firstPercent) / (slides - 1);
        }
        else
        {
            timeFirstSlide = totalTime;
        }

        yield return new WaitForSeconds(1f);

        yield return FillLevelFlash(expAmount);

        yield return new WaitForSeconds(1f);

        yield return Fade();

        Draining = false;
        gameObject.SetActive(false);
    }

    private IEnumerator FillLevelFlash(int amount)
    {
        int num = 0;
        while (levelInfo.Experience + amount >= Level.ExpToNextLevel)
        {
            int diffToLvl = Level.ExpToNextLevel - levelInfo.Experience;
            Level.AddExperience(diffToLvl);
            amount -= diffToLvl;

            levelInfo = Level.GetLevelInfo();

            StartCoroutine(DrainExp(amount + diffToLvl, diffToLvl, num++ == 0 ? timeFirstSlide : timePerSlide));
            yield return FillToAmount(1, num++ == 0 ? timeFirstSlide : timePerSlide);
            levelFlash.fillAmount = 0;

            levelText.text = levelInfo.Level.ToString();
        }

        Level.AddExperience(amount);
        levelInfo = Level.GetLevelInfo();

        StartCoroutine(DrainExp(amount, amount, num == 0 ? timeFirstSlide : timePerSlide));
        yield return FillToAmount((float)levelInfo.Experience / (float)Level.ExpToNextLevel, num == 0 ? timeFirstSlide : timePerSlide);

        yield return null;
    }

    private IEnumerator DrainExp(int startAmount, int amountLess, float time)
    {
        float t = 0;

        while (t <= 1)
        {
            expText.text = Mathf.RoundToInt(startAmount - amountLess * t).ToString();
            t += Time.deltaTime / time;
            yield return null;
        }

        expText.text = Mathf.RoundToInt(startAmount - amountLess).ToString();
    }

    // Only positive
    private IEnumerator FillToAmount(float fillTarget, float time)
    {
        float currentFillAmount = levelFlash.fillAmount;

        float t = 0;

        while (t <= 1)
        {
            levelFlash.fillAmount = currentFillAmount * (1 - t) + fillTarget * t;

            t += (float)Time.deltaTime / (float)time;
            yield return null;
        }

        levelFlash.fillAmount = fillTarget;
    }
    private IEnumerator Fade()
    {
        float t = 1;

        while (t >= 0)
        {
            t -= Time.deltaTime * fadeSpeed;

            canvasRenderer.alpha = t;

            yield return null;
        }

        canvasRenderer.alpha = 0;
    }
}
