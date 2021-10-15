using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterDelay : MonoBehaviour
{
    [SerializeField]
    private float timeToLive = 1f;

    [SerializeField]
    private bool shouldDestroy = false;

    private float timer = 0;

    private void Start()
    {
        if (shouldDestroy)
        {
            Destroy(gameObject, timeToLive);
        }
    }

    private void Update()
    {
        if (!shouldDestroy)
        {
            timer += Time.deltaTime;
            if (timer >= timeToLive)
            {
                timer = 0;
                gameObject.SetActive(false);
            }
        }
    }
}
