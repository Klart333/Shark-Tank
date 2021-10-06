using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScoreBoardMode : MonoBehaviour
{
    [Header("Highscores")]
    [SerializeField]
    private TextMeshProUGUI names;
    [SerializeField]
    private TextMeshProUGUI scores;

    [Header("Local")]
    [SerializeField]
    private TextMeshProUGUI localText;

    [SerializeField]
    private Image localImage;

    [Header("Global")]
    [SerializeField]
    private TextMeshProUGUI globalText;

    [SerializeField]
    private Image globalImage;

    private LocalHighscoreList localHighscores;
    private GlobalHighscores globalHighscores;

    private void Start()
    {
        localHighscores = FindObjectOfType<LocalHighscoreList>();
        globalHighscores = FindObjectOfType<GlobalHighscores>();
    }

    public void SetLocal()
    {
        localText.color = Color.white;
        localImage.color = Color.black;

        globalText.color = Color.black;
        globalImage.color = Color.white;

        names.text = "";
        scores.text = "";
        localHighscores.ShowScores();
    }

    public void SetGlobal()
    {
        globalText.color = Color.white;
        globalImage.color = Color.black;

        localText.color = Color.black;
        localImage.color = Color.white;

        names.text = "";
        scores.text = "";
        SetScoresToGlobal(globalHighscores.Highscores);
    }

    public void SetScoresToGlobal(Highscore[] highscores)
    {
        for (int i = 0; i < 9; i++) 
        {
            if (string.IsNullOrEmpty(highscores[i].username))
            {
                names.text += "N/A\n";
                continue;
            }

            names.text += highscores[i].username + "\n"; 
        }

        for (int i = 0; i < 9; i++) // Same here
        {
            scores.text += highscores[i].score + "\n";
        }
    }
}
