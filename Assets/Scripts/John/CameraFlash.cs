using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlash : MonoBehaviour, IClickable
{
    [SerializeField]
    private SimpleAudioEvent cameraFlashSFX;

    private SharkSpawner sharkSpawner;

    public event Action OnFlash = delegate { };

    public int cameraFlashes = 3;

    private void Awake()
    {
        OnFlash = delegate { };
        sharkSpawner = FindObjectOfType<SharkSpawner>();

        var cam = Camera.main;
        transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.15f, 0)) - new Vector3(0, 0, -9);
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarted += Instance_OnGameStarted;
    }

    private void Instance_OnGameStarted()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        MoreCameraCard camCard = FindObjectOfType<MoreCameraCard>();
        if (camCard != null && camCard.IsActive)
        {
            cameraFlashes += camCard.AdditionalCameraFlashes;
        }
    }

    public void OnClicked()
    {
        if (cameraFlashes > 0)
        {
            Flash();
        }
    }

    private void Flash()
    {
        AudioManager.Instance.PlaySoundEffect(cameraFlashSFX); // Plays the flash sound, doesn't necessarily need a mixer group 

        Shark[] sharks = FindObjectsOfType<Shark>();

        foreach (Shark shark in sharks)
        {
            shark.gameObject.SetActive(false); // Sends every shark on the screen back to its pool
        }

        cameraFlashes--;
        OnFlash(); // Calls the event, so that every connected script does its part

        StartCoroutine(DeactivateSharkSpawnerForDelay(3)); 
    }

    private IEnumerator DeactivateSharkSpawnerForDelay(float delay) // Deactivates the sharkspawner for delay seconds
    {
        sharkSpawner.gameObject.SetActive(false);

        yield return new WaitForSeconds(delay);

        sharkSpawner.gameObject.SetActive(true);
    }
}
