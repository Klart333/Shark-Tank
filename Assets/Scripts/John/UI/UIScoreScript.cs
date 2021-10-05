using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIScoreScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private UIResponseText responseText;

    [SerializeField]
    private int baseValueForSharks = 12; // Try to get different values for different sharks

    public int Score { get { return score; } } // Protects score so that only his clone big brother can be accesed, read only

    private int score;

    private CameraShake cameraScript;

    private void Start()
    {
        cameraScript = Camera.main.GetComponent<CameraShake>();
        GameManager.Instance.OnSharkKilled += IncreaseScore;
    }

    private void UpdateScore()
    {
        scoreText.text = Score.ToString();
    }

    private void IncreaseScore(float sharkTimeToKill)
    {
        int value = baseValueForSharks - Mathf.RoundToInt(sharkTimeToKill);
        value = Mathf.RoundToInt((float)value * (1 + GameManager.Instance.hitSpree / 10));

        if (CheckForGunHits(sharkTimeToKill, value) == true)
        {
            AddAndUpdateScore(value);
            return;
        }


        if (sharkTimeToKill < GameManager.Instance.doubleTime) 
        {
            value = MultiplyEffect(value, 2f, "Double!");
            AddAndUpdateScore(value);

            return;
        }


        AddAndUpdateScore(value);
    }

    private bool CheckForGunHits(float sharkTimeToKill, int value)
    {
        if (PowerupManager.Instance.gunActive && sharkTimeToKill < 0.75f)
        {
            MultiplyEffect(value, 6f, "Sextuple!");
            return true;
        }
        else if (PowerupManager.Instance.gunActive)
        {
            MultiplyEffect(value, 3f, "Triple!");
            return true;
        }

        return false;
    }

    private int MultiplyEffect(int value, float multiplicativeValue, string response)
    {
        value = Mathf.RoundToInt(value * multiplicativeValue);
        responseText.PrintDef(response);

        Vector2 randomShakeFrequency = new Vector2(Random.Range(0, 100) * multiplicativeValue, Random.Range(0, -100) * multiplicativeValue / 2);
        Vector2[] parms = new Vector2[] { new Vector2(0.05f * multiplicativeValue, 0.05f * multiplicativeValue), randomShakeFrequency, new Vector2(10, 0) };
        cameraScript.StartCoroutine("ScreenShakeSinWave", parms);

        return value;
    }
    private void AddAndUpdateScore(int value)
    {
        score += value;
        UpdateScore();
    }
}
