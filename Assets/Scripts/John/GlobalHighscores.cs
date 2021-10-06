using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GlobalHighscores : MonoBehaviour
{
    const string privateCode = "wdOhgCTookWV62W2R4dgjQGGyKo1SRq0akni11I3MLvw";
    const string publicCode = "615dff668f40bb0e288cc0f4";
    const string webURL = "http://dreamlo.com/lb/";

    [SerializeField]
    private bool downloadable = true;

    public Highscore[] Highscores { get; private set; }

    private void Awake()
    {
        if (downloadable)
        {
            DownloadHighscores();
        }
    }

    public void AddNewHighscore(string username, int score)
    {
        StartCoroutine(UploadNewHighscore(username, score));
    }

    private IEnumerator UploadNewHighscore(string username, int score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Succesful");
        }
        else
        {
            print("Error uploading " + www.error);
        }
    }

    public void DownloadHighscores()
    {
        StartCoroutine(DownloadHighscoresFromDatabase());
    }

    private IEnumerator DownloadHighscoresFromDatabase()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighscores(www.text);
        }
        else
        {
            print("Error uploading " + www.error);
        }
    }

    private void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        Highscores = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);

            Highscores[i] = new Highscore(username, score);
        }
    }
}

public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}
