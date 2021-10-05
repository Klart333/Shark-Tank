using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    private SharkKilledParticleSystem sharkKillParticleSystem;

    [SerializeField]
    private ClickParticle clickParticle;

    private void Start()
    {
        GameManager.Instance.OnSharkKilled += PlayParticleSystemOnSharkKilled;
    }

    private void PlayParticleSystemOnSharkKilled(float sharkTimeToKill)
    {
        Vector3 clickPos = Input.mousePosition;
        Vector3 position = Camera.main.ScreenToWorldPoint(clickPos);

        sharkKillParticleSystem.GetAtPosAndRot<SharkKilledParticleSystem>(position, Quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickParticle.GetAtPosAndRot<ClickParticle>(GetWorldPointClicked(), Quaternion.identity);
        }
    }

    private Vector3 GetWorldPointClicked()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f); // I don't understand why z has to be 10, and it has to be 10
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    
}
