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

    public void PlaySoundEffect(AudioClip clip)
    {
        if (audioSource.isPlaying)
            PlaySecondSoundEffect(clip);
        else
        {
            audioSource.clip = clip;

            audioSource.Play();
        }
    }

    public void PlayRepeatingSoundEffect(AudioClip clip)
    {
        repeatingAudioSource.clip = clip;

        repeatingAudioSource.Play();
    }


    private void PlaySecondSoundEffect(AudioClip clip)
    {
        secondAudioSource.clip = clip;

        secondAudioSource.Play();
    }
}
