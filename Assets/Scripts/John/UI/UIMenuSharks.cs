using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuSharks : MonoBehaviour
{
    [SerializeField]
    private GameObject shark;

    [Header("Spawning")]
    [SerializeField]
    private float spawnRate = 1f;

    [SerializeField]
    private float sharkLiveTime = 15;

    [SerializeField]
    private float sharkSpeed = 1f;

    private List<GameObject> spawnedSharks = new List<GameObject>();
    private List<float> spawnedTimes = new List<float>();

    private new Camera camera;

    private float spawnTimer = 0;

    private float timerMax
    {
        get
        {
            return 1.0f / spawnRate;
        }
    }

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= timerMax)
        {
            spawnTimer = 0;

            SpawnShark();
        }

        for (int i = 0; i < spawnedSharks.Count; i++)
        {
            if (Time.time - spawnedTimes[i] >= sharkLiveTime)
            {
                spawnedSharks.RemoveAt(i);
                spawnedTimes.RemoveAt(i);
            }
        }

        for (int i = 0; i < spawnedSharks.Count; i++)
        {
            spawnedSharks[i].transform.position += spawnedSharks[i].transform.right * -sharkSpeed;
        }
    }

    private void SpawnShark()
    {
        Vector2 randomPos = RandomScreenEdgePoint();

        var shrk = Instantiate(shark, this.transform);
        (shrk.transform as RectTransform).position = randomPos;

        if (randomPos.x < 0)
        {
            shrk.transform.Rotate(new Vector3(0, 180, 0));
        }

        spawnedSharks.Add(shrk);
        spawnedTimes.Add(Time.time);
    }

    private Vector2 RandomScreenEdgePoint()
    {
        // The screen is 1920 by 1080, remove some for margin, except for on the x where we want the max or min
        float xPos = UnityEngine.Random.Range(1, 3) == 1 ? -200 : camera.pixelWidth + 200; // Basically a coin flip, 3 is not included 
        float yPos = UnityEngine.Random.Range(camera.pixelHeight * 0.3f, camera.pixelHeight * 0.97f);

        return new Vector2(xPos, yPos); 
    }
}
