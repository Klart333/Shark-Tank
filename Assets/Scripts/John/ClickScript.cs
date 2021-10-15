using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickScript : MonoBehaviour
{
    [SerializeField]
    private SimpleAudioEvent punchSFX;

    [SerializeField]
    private Missile missilePrefab;

    private UIHitSpree hitSpreeScript;
    private AudioSource audioSource;
    private ClickHitboxCard clickCard;
    private MissileCard missileCard;

    private float radMultiplier = 1f;
    private float chanceForMissile = -1;
    private int missileAmount = 0;


    private void Start()
    {
        hitSpreeScript = FindObjectOfType<UIHitSpree>();
        audioSource = GetComponent<AudioSource>();

        GameManager.Instance.OnGameStarted += Instance_OnGameStarted;
    }

    private void Instance_OnGameStarted()
    {
        clickCard = FindObjectOfType<ClickHitboxCard>();
        if (clickCard != null && clickCard.IsActive)
        {
            radMultiplier = clickCard.ClickRadiusMultiplier;
        }

        missileCard = FindObjectOfType<MissileCard>();
        if (missileCard != null && missileCard.IsActive)
        {
            chanceForMissile = missileCard.MissileSpawnPercent;
            missileAmount = missileCard.MissileAmount;
        }
    }

    void Update()
    {
        if (ShouldClick())
        {
            punchSFX.Play(audioSource); // Whether he hit or miss, works with the gun because this script is disabled
            if (Click() == true) // If we hit something
            {
                float randnum = UnityEngine.Random.Range(0.0f, 1.0f);
                if (randnum <= chanceForMissile)
                {
                    SpawnMissiles();
                }
            }
            else
            {
                GameManager.Instance.hitSpree = 0;
                hitSpreeScript.UpdateHitSpree();
            }

        }
    }

    private bool ShouldClick()
    {
        return (Input.GetMouseButtonDown(0) && !GameManager.Instance.Frozen && GameManager.Instance.GameStarted);
    }

    private bool Click()
    {
        Vector3 position = GetWorldPointClicked();

        IClickable target = null;
        target = TryClickAtPosition(position);

        return (target != null); // We hit something if target isn't null
    }

    private Vector3 GetWorldPointClicked()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f); // I don't understand why z has to be 10, and it has to be 10
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    private IClickable TryClickAtPosition(Vector3 position)
    {
        IClickable target = null;

        Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f * radMultiplier);

        foreach (Collider hit in hitColliders)
        {
            target = hit.GetComponent<IClickable>();
            if (target != null)
            {
                target.OnClicked();
            }
        }

        return target;
    }

    private void SpawnMissiles()
    {
        Vector3 position = GetWorldPointClicked();
        for (int i = 0; i < missileAmount; i++)
        {
            print("Spawning");
            Vector3 random = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), 0);
            missilePrefab.GetAtPosAndRot<Missile>(position + (random / 2.0f), Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0, 360))));
        }
    }
}
