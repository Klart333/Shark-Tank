using System;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour 
{
    public static Audio Instance;

    [SerializeField]
    private SimpleAudioEvent doubleSFX;

    private AudioSource audioSource;

    private float stackingDoubleTimer;
    private float stackingPitch = 0.9f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameManager.Instance.OnSharkKilled += AudioOnSharkKilled;
    }
    private void Update()
    {
        stackingDoubleTimer += Time.deltaTime;

        if (stackingDoubleTimer >= GameManager.Instance.doubleTime) // Resets the stacking pitch if too much time passes 
        {
            stackingDoubleTimer = 0;
            stackingPitch = 0.9f;
        }
    }
    public void AudioOnSharkKilled(float sharkTimeToKill)
    {
        if (sharkTimeToKill < GameManager.Instance.doubleTime)
        {
            PlaySoundEffect(doubleSFX, stackingPitch += 0.1f); // Increases the pitch everytime
            stackingDoubleTimer = 0;
            return;
        }
    }

    public void PlaySoundEffect(SimpleAudioEvent audioEvent, float pitch = 1)
    {
        AudioMixerGroup audioMixerGroup = audioEvent.Mixer;

        if (audioMixerGroup != null)
        {
            audioMixerGroup.audioMixer.SetFloat("DoubleEffectPitch", pitch);
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        audioEvent.Play(this.audioSource);
    }
}
