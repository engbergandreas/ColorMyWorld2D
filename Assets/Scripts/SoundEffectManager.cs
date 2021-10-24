using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;
    [SerializeField] private AudioSource audioSource, secondAudioSource, repeatingAudioSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySoundEffect(AudioClip clip, float volume = 1.0f)
    {
        if (audioSource.isPlaying)
            PlaySecondSoundEffect(clip);
        else
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    public void PlayRepeatingSoundEffect(AudioClip clip, float volume = 1.0f)
    {
        repeatingAudioSource.clip = clip;
        audioSource.volume = volume;
        repeatingAudioSource.Play();
    }


    private void PlaySecondSoundEffect(AudioClip clip , float volume = 1.0f)
    {
        secondAudioSource.clip = clip;
        audioSource.volume = volume;
        secondAudioSource.Play();
    }
}
