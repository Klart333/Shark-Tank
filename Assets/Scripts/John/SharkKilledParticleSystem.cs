using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkKilledParticleSystem : PooledMonoBehaviour
{
    public Color Color;

    public Dictionary<string, Color> SharkColor = new Dictionary<string, Color>()
    {
        {"OGShark", new Color32(186, 222, 238, 255) },
        {"CyanShark", new Color32(173, 255, 255, 255) },
        {"RedShark", new Color32(235, 17, 19, 255) },
        {"GreenShark", new Color32(177, 255, 179, 255) },
        {"WhiteShark", new Color32(255, 255, 255, 255) },
        {"GoldShark", new Color32(255, 238, 38, 255) },
        {"AntiShark", new Color32(85, 14, 10, 255) }
    };

    private new ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();

        Color color;
        if (SharkColor.TryGetValue(PlayerPrefs.GetString("ActiveShark"), out color))
        {
            var main = particleSystem.main;
            main.startColor = color;
        }

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
