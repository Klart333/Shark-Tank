using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio Events/Simple")]
public class SimpleAudioEvent : ScriptableObject
{
    [SerializeField]
    private AudioClip[] clips = new AudioClip[0];

    [SerializeField]
    private RangedFloat volume = new RangedFloat(1, 1);

    [SerializeField]
    [MinMaxRange(0f, 2f)]
    private RangedFloat pitch = new RangedFloat(1, 1);

    public AudioMixerGroup Mixer;

    public void Play(AudioSource source)
    {
        source.outputAudioMixerGroup = Mixer; // Can be null

        int clipIndex = UnityEngine.Random.Range(0, clips.Length);
        source.clip = clips[clipIndex];

        source.pitch = UnityEngine.Random.Range(pitch.minValue, pitch.maxValue);
        source.volume = UnityEngine.Random.Range(volume.minValue, volume.maxValue);

        source.Play();
    }
}
