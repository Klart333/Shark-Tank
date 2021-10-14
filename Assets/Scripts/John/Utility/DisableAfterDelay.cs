using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterDelay : MonoBehaviour
{
    [SerializeField]
    private float timeToLive = 1f;

    private void Start()
    {
        Destroy(gameObject, timeToLive);
    }
}
