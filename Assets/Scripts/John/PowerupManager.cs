using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour // Didn't amount to much, but that's mainly because the gun is the only power up
{
    public static PowerupManager Instance;

    public bool gunActive = false; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
