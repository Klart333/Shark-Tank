using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkSpawner : MonoBehaviour
{
    [SerializeField]
    private Shark[] prefabs; // Array of shark prefabs, accessed through the script

    private float spawnTimer;
    private int sortingLayerNum = 0;
    private new Camera camera;

    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            return;
        }

        spawnTimer += Time.deltaTime;
        if (ShouldSpawn()) 
        {
            spawnTimer = 0;
            SpawnShark();
        }
    }

    private bool ShouldSpawn()
    {
        float spawnTime;
        if (GameManager.Instance.DifficultyMultiplier <= 5)
        {
            spawnTime = Random.Range(2, 5) / GameManager.Instance.DifficultyMultiplier;
        }
        else
        {
            spawnTime = Random.Range(0.4f, 0.6f) / Mathf.Log10(GameManager.Instance.DifficultyMultiplier);
        }

        return (spawnTimer >= spawnTime) && (GameManager.Instance.Gameover == false);
    }

    private void SpawnShark()
    {
        Vector3 position = RandomScreenEdgeToWorldPoint();
        Shark prefab = GetActiveShark();

        Shark shark = prefab.GetAtPosAndRot<Shark>(position, prefab.gameObject.transform.rotation); // We call the inherited method 'Get' which asks the Pool for a GameObject from the queue and then makes it active
        shark.SetRoamGoal();

        shark.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerNum--;
    }

    private Shark GetActiveShark()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("ActiveShark")))
        {
            PlayerPrefs.SetString("ActiveShark", "OGShark");
            return prefabs[0];
        }

        Shark prefab = null;
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].name == PlayerPrefs.GetString("ActiveShark"))
            {
                prefab = prefabs[i];

                if (prefabs[i].name == "PumpShark")
                {
                    GameManager.Instance.SetExtraDifficulty(2f);
                }
            }
        }

        if (prefab != null)
        {
            return prefab;
        }
        else
        {
            print("Active Shark Not Found");
            return null;
        }
    }

    private Vector3 RandomScreenEdgeToWorldPoint()
    {
        // The screen is 1920 by 1080, remove some for margin, except for on the x where we want the max or min
        float xPos = Random.Range(1, 3) == 1 ? xPos = -100f : xPos = camera.pixelWidth + 100; // Basically a coin flip, 3 is not included 
        float yPos = Random.Range(100, camera.pixelHeight - 100);
        if (xPos == camera.pixelWidth + 100) // The shark can't be allowed spawn over the camera
        {
            yPos = Random.Range((float)camera.pixelHeight * 0.30f, (float)camera.pixelHeight * 0.8f);
        }

        Vector2 randomScreenPos = new Vector2(xPos, yPos); 
        Vector3 position = Camera.main.ScreenToWorldPoint(randomScreenPos);
        position.z = 0;
        return position;
    }
}
