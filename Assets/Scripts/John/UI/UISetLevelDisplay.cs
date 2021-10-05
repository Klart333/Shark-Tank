using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UISetLevelDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI levelRankText;

    [SerializeField]
    private Image levelFlashFill;

    private Dictionary<int, string> levelRanks = new Dictionary<int, string>()
    {
        {0, "Shark Bait" },
        {10, "Fish out of Water" },
        {20, "Sea Stone" },
        {30, "Salmon" },
        {40, "Dolphin" },
        {50, "Shark" },
        {60, "Great White Shark" },
        {70, "Shark Tanker" },
        {80, "Shark Killer" },
        {90, "Shark Calamity" },
        {100, "Shark God" }
    };

    private void Start()
    {
        LevelInfo levelInfo = Level.GetLevelInfo();
        
        levelText.text = levelInfo.Level.ToString();

        int roundedLevel = Mathf.RoundToInt(levelInfo.Level / 10.0f) * 10;
        string rankName = "";
        if (roundedLevel > 90)
        {
            levelRanks.TryGetValue(100, out rankName);
            levelRankText.text = rankName;
        }
        else
        {
            levelRanks.TryGetValue(roundedLevel, out rankName);
            levelRankText.text = rankName;
        }

        levelFlashFill.fillAmount = (float)levelInfo.Experience / (float)Level.ExpToNextLevel;
    }
}
