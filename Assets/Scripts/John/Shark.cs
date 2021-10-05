using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Shark : PooledMonoBehaviour, IClickable // I realise in hindsight that i probably could have used a blend tree for the animation
{
    [SerializeField]
    private SimpleAudioEvent sharkBite;

    [SerializeField]
    private float totalTimeBeforeChomp = 8; // The time is divided over the amount of sharkPhases

    [SerializeField]
    private int sharkBitePhases = 5;
    
    [SerializeField]
    private float startMoveSpeed = 3;

    [SerializeField]
    private float sharkGrowSpeed = 0.5f;

    private AudioSource audioSource;
    private Animator animator;
    private UIHitSpree hitSpreeScript;

    private float killTimer;
    private bool moving = true;
    private bool inPosition = false;

    private bool[] sharkTurning;
    private float goal;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        hitSpreeScript = GameObject.Find("[TextManager]").GetComponent<UIHitSpree>();
        animator = GetComponent<Animator>();
        sharkTurning = new bool[3];
    }
    private void Update()
    {
        killTimer += Time.deltaTime;

        if (GameManager.Instance.Gameover && moving)
        {
            moving = false;
            StopCoroutine("MoveToX");
        }
        else if (!GameManager.Instance.Gameover && !moving && !inPosition)
        {
            moving = true;
            StartCoroutine(MoveToX(goal));
        }

        if (inPosition)
        {
            Grow();
        }
    }
    private void Grow()
    {
        if (GameManager.Instance.Gameover) // The sharks stop to give the illusion of the timeScale being set to 0
            return;


        transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime * sharkGrowSpeed;
    }
    public void SetRoamGoal()
    {
        goal = GetRandomXPos();

        StartCoroutine("MoveToX", goal);
    }

    private IEnumerator MoveToX(float xGoal)
    {
        bool goingLeft = true;

        if (transform.position.x < 0)
        {
            goingLeft = false;
        }

        float scaledMoveSpeed = (Mathf.Log10(GameManager.Instance.DifficultyMultiplier) < 1 ? 1 : Mathf.Log10(GameManager.Instance.DifficultyMultiplier)); // If the multiplier is under 1 take 1 else take the multiplier

        if (goingLeft)
        {
            while (transform.position.x > xGoal)
            {
                transform.position += Vector3.left * startMoveSpeed * Time.deltaTime * scaledMoveSpeed; // Scaled with difficulty Multiplier
                yield return new WaitForSeconds(0.001f);
                TurnAtShortDistance(xGoal);
            }
            inPosition = true;
            moving = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            while (transform.position.x < xGoal)
            {
                transform.position += Vector3.right * startMoveSpeed * Time.deltaTime * scaledMoveSpeed;
                yield return new WaitForSeconds(0.001f);
                TurnAtShortDistance(xGoal);
            }
            inPosition = true;
            moving = false;
        }

        StartCoroutine("SharkBitePhases"); // When we are in position we start the SharkBitePhases
    }

    private void TurnAtShortDistance(float xGoal)
    {
        if (Mathf.Abs(transform.position.x - xGoal) <= 2 && !sharkTurning[0])
        {
            sharkTurning[0] = true;
            animator.SetTrigger("SharkTurn1");
        }

        if (Mathf.Abs(transform.position.x - xGoal) <= 1 && !sharkTurning[1])
        {
            sharkTurning[1] = true;
            animator.SetTrigger("SharkTurn2");
        }

        if (Mathf.Abs(transform.position.x - xGoal) <= 0.5f && !sharkTurning[2])
        {
            sharkTurning[2] = true;
            animator.SetTrigger("SharkTurn3");
        }
    }


    private float GetRandomXPos()
    {
        float screenXPos = UnityEngine.Random.Range(100, Camera.main.pixelWidth - 100);

        Vector3 randomPos = Camera.main.ScreenToWorldPoint(new Vector3(screenXPos, 0, 0));
        return randomPos.x;
    }

    public void OnClicked()
    {
        GameManager.Instance.hitSpree++;
        hitSpreeScript.UpdateHitSpree();
        GameManager.Instance.SharkKilled(killTimer);

        ReturnToPool(); // Returns the shark to the pool
    }

    private IEnumerator SharkBitePhases()
    {
        float timeBetweenPhases = totalTimeBeforeChomp / sharkBitePhases;

        for (int i = 1; i <= sharkBitePhases; i++)
        {
            while (GameManager.Instance.Gameover)
            {
                yield return null;
            }

            animator.SetTrigger("SharkBite" + i.ToString());

            yield return new WaitForSeconds(timeBetweenPhases);
            if (i == sharkBitePhases)
            {
                StartCoroutine("Bite");
            }
        }
    }

    private IEnumerator Bite()
    {
        animator.SetTrigger("Gameover");
        sharkBite.Play(audioSource); 

        GameManager.Instance.Gameover = true;
        GameManager.Instance.Frozen = true;

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.GameOver(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        ResetShark();
    }
    private void ResetShark()
    {
        StopCoroutine("SharkBitePhases");
        animator.SetTrigger("SharkRoam");

        gameObject.transform.localScale = Vector3.one;

        killTimer = 0;

        inPosition = false;
        moving = true;

        for (int i = 0; i < sharkTurning.Length; i++)
        {
            sharkTurning[i] = false;
        }
    }

}
