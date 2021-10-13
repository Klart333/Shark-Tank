using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour 
{
    public static AudioManager Instance;

    [SerializeField]
    private SimpleAudioEvent doubleSFX;

    [SerializeField]
    private SimpleAudioEvent clickSound;

    private AudioSource audioSource;

    private Queue<AudioSource> audioSources = new Queue<AudioSource>();

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

    public void PlaySoundEffect(SimpleAudioEvent audio, float pitch = 1)
    {
        if (audioSources.Count == 0)
        {
            audioSources.Enqueue(gameObject.AddComponent<AudioSource>());
        }
        var source = audioSources.Dequeue();

        AudioMixerGroup audioMixerGroup = audio.Mixer;

        if (audioMixerGroup != null)
        {
            audioMixerGroup.audioMixer.SetFloat("DoubleEffectPitch", pitch);
            source.outputAudioMixerGroup = audioMixerGroup;
        }
        
        int index = audio.Play(source);

        StartCoroutine(ReturnToQueue(source, audio.Clips[index].length));
    }

    private IEnumerator ReturnToQueue(AudioSource source, float length)
    {
        yield return new WaitForSeconds(length);

        audioSources.Enqueue(source);
    }

    public void PlayClickSound()
    {
        PlaySoundEffect(clickSound);
    }
}
