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

    private LevelInfo levelInfo;

    private float fillSpeed = 1f;
    private float fadeSpeed = 0.6f;

    private bool stupidDraining = false;

    private void Update()
    { 
        if (!stupidDraining)
        {
            int n = 0;
            int.TryParse(FindObjectOfType<ScorePanele>().GetComponent<TextMeshProUGUI>().text, out n);
            StartCoroutine(StartDrain(n));
        }
    }

    public IEnumerator StartDrain(int amount)
    {
        levelInfo = Level.GetLevelInfo();

        stupidDraining = true;
        Draining = true;

        expText.text = string.Format("Exp: {0}", amount);
        levelText.text = levelInfo.Level.ToString();
        levelFlash.fillAmount = (float)levelInfo.Experience / (float)Level.ExpToNextLevel;
        
        yield return new WaitForSeconds(1f);

        yield return FillLevelFlash(amount);
        yield return new WaitForSeconds(1f);

        yield return Fade();

        Draining = false;
        gameObject.SetActive(false);
    }

    private IEnumerator DrainExp(int startAmount, int amountLess, float speed)
    {
        float t = 0;

        while (t <= 1)
        {
            expText.text = Mathf.RoundToInt(startAmount - amountLess * t).ToString();
            t += Time.deltaTime * speed;
            yield return null;
        }

        expText.text = Mathf.RoundToInt(startAmount - amountLess).ToString();
    }

    private IEnumerator FillLevelFlash(int amount)
    {
        while (levelInfo.Experience + amount > Level.ExpToNextLevel)
        {
            int diffToLvl = Level.ExpToNextLevel - levelInfo.Experience;
            Level.AddExperience(diffToLvl);
            amount -= diffToLvl;

            levelInfo = Level.GetLevelInfo();
            fillSpeed = 1250.0f / (float)diffToLvl <= 0.2f ? 0.2f : 1250.0f / (float)diffToLvl;

            StartCoroutine(DrainExp(amount + diffToLvl, diffToLvl, fillSpeed));
            yield return FillToAmount(1);
            levelFlash.fillAmount = 0;

            levelText.text = levelInfo.Level.ToString();
        }

        Level.AddExperience(amount);
        levelInfo = Level.GetLevelInfo();

        fillSpeed = 1250.0f / (float)amount <= 0.2f ? 0.2f : 1250.0f / (float)amount;
        StartCoroutine(DrainExp(amount, amount, fillSpeed));
        yield return FillToAmount((float)levelInfo.Experience / (float)Level.ExpToNextLevel);

        yield return null;
    }

    // Only positive
    private IEnumerator FillToAmount(float fillTarget)
    {
        float currentFillAmount = levelFlash.fillAmount;

        float t = 0;

        while (t <= 1)
        {
            levelFlash.fillAmount = currentFillAmount * (1 - t) + fillTarget * t;

            t += Time.deltaTime * fillSpeed;
            yield return null;
        }

        levelFlash.fillAmount = fillTarget;
    }
    private IEnumerator Fade()
    {
        Image[] images = GetComponentsInChildren<Image>();
        Color[] colorList = new Color[8];
        for (int i = 0; i < images.Length; i++)
        {
            colorList[i] = images[i].color;
        }
        colorList[6] = (expText.color);
        colorList[7] = (levelText.color);

        Color[] ogColorList = colorList;

        float t = 1;

        while (t >= 0)
        {
            for (int i = 0; i < images.Length; i++)
            {
                colorList[i].a *= t;
                images[i].color = colorList[i];
            }
            colorList[6].a *= t;
            expText.color = colorList[6];
            colorList[7].a *= t;
            levelText.color = colorList[7];

            for (int i = 0; i < colorList.Length; i++)
            {
                colorList[i].a = ogColorList[i].a;
            }

            t -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        
    }
}
