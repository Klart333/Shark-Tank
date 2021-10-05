using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffect : MonoBehaviour
{
    [SerializeField]
    protected SimpleAudioEvent soundEffect;

    [SerializeField]
    private bool playOnAwake;

    protected AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (playOnAwake)
        {
            PlaySoundEffect();
        }
    }

    public void PlaySoundEffect()
    {
        soundEffect.Play(audioSource);
    }
}
