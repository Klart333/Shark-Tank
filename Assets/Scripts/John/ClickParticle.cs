using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticle : PooledMonoBehaviour
{
    private new ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        particleSystem.Play();
        StartCoroutine("DeactivateAfterTime");
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(particleSystem.main.startLifetime.constantMax);

        gameObject.SetActive(false);
    }
}
