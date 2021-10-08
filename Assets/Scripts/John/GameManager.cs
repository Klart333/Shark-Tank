using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class GameManager : MonoBehaviour // The GameManager ties all the seperate scripts together, thus there is only one (well not all but most)
{ 
    public static GameManager Instance;

    public bool GameStarted { get; private set; }

    [SerializeField]
    private float difficultyIncreasedFromSharkKill = 0.1f; // Rather long than ununderstanble, right?

    [SerializeField]
    private float startDifficulty = 1;

    [SerializeField]
    public float doubleTime = 1.0f;

    public event Action<float> OnSharkKilled = delegate { }; // Initialises the event into a empty delegate so that it doesn't blow up if it's empty

    public bool Gameover = false;
    public bool Frozen = false;

    [HideInInspector]
    public int hitSpree = 0;

    public float DifficultyMultiplier { get; private set; }

    private Shark killerShark;
    private int localScore; // Only used for passing into the SetScore, fake hence called local
    private bool switchingScene = false;
    private bool watchedVideo = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            SceneManager.activeSceneChanged += ActiveSceneChanged; // The ActiveSceneChanged method wont call when we enter the gamescene for the first time because the event needs be registered

        }
        else // Don't want two, a problem when we return to the gamescene and there is a GameManager there
        {
            Destroy(gameObject);
        }

        OnSharkKilled += IncreaseDifficultyOnSharkKill;
    }


    private void Start()
    {
        DifficultyMultiplier = startDifficulty;
    }

    public void StartGame()
    {
        GameStarted = true;
    }
    private void ActiveSceneChanged(Scene currentScene, Scene nextScene)
    {
        FindObjectOfType<FadePanel>().StartCoroutine(FindObjectOfType<FadePanel>().FadeIn());

        if (nextScene.buildIndex == 1) // When we enter the gamescene we want to reset
        {
            hitSpree = 0;
            DifficultyMultiplier = startDifficulty;
            Gameover = false;
            GameStarted = false;
            Frozen = false;
            watchedVideo = false;

            Pool.dictionaryPools = new Dictionary<PooledMonoBehaviour, Pool>(); // Removes the stored Pools

            OnSharkKilled = delegate { }; // Resets the event, so that it doesn't have old functions with messed up references 
            
            OnSharkKilled += Audio.Instance.AudioOnSharkKilled; // Adds these back
            OnSharkKilled += IncreaseDifficultyOnSharkKill; 
        }

        if (nextScene.buildIndex == 2) // If the scene is the gameover scene we need to pass in the score
        {
            SetGameoverScore(localScore);
            SaveScore.SaveGame(localScore, PlayerPrefs.GetString("PlayerName"));
            FindObjectOfType<GlobalHighscores>().AddNewHighscore(string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")) ? "N/A" : PlayerPrefs.GetString("PlayerName"), localScore);


            FindObjectOfType<UIDrainExp>().StartCoroutine(FindObjectOfType<UIDrainExp>().StartDrain(localScore));
        }
    }

    public void SharkKilled(float timeToKill)
    {
        OnSharkKilled(timeToKill);
    }

    private void IncreaseDifficultyOnSharkKill(float timer)
    {
        DifficultyMultiplier += difficultyIncreasedFromSharkKill;
    }

    public void GameOver(Shark sharkThatIsBiting)
    {
        sharkThatIsBiting.GetComponent<Animator>().SetTrigger("Frozen");

        if (switchingScene)
        {
            sharkThatIsBiting.OnClicked();
            return;
        }
        switchingScene = true;

        killerShark = sharkThatIsBiting;
        localScore = FindObjectOfType<UIScoreScript>().Score;

        if (watchedVideo)
        {
            StartCoroutine(SwitchSceneAfterDelay(0.5f));
        }
        else
        {
            FindObjectOfType<UIRevive>().ToggleRevive(true);
        }
    }

    public IEnumerator ContinueGame()
    {
        watchedVideo = true;

        if (killerShark != null)
        {
            killerShark.OnClicked();
        }

        yield return new WaitForSeconds(3);

        Gameover = false;
        switchingScene = false;
    }

    public IEnumerator SwitchSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(2);
        switchingScene = false;
    }

    private void SetGameoverScore(int score)
    {
        FindObjectOfType<ScorePanele>().SkrivPoeng(score);
    }
}
