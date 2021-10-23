using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class DoorButton : MonoBehaviour
{

    public float timeUntilClosed = 5;
    public bool playSound = true, alwaysOpen = false;

    public GameObject closedDoor, openDoor;

    public DrawableObject button;

    public AudioSource audioSource1, audioSource2;
    
    public AudioMixer audioMixer;

    public AudioClip doorClosedSoundEffect;
    
    private bool open = false, warningSoundPlayed = false;
    private float timer = 0;


    private void Start()
    {
        button._event.AddListener(OnFullyColoredButton);
        if (!alwaysOpen)
            Player.Instance.deathEvent.AddListener(CloseDoor);
        CloseDoor();
    }

    private void Update()
    {
        if (alwaysOpen)
            return;

        if (open)
        {
            timer -= Time.deltaTime;
            if (playSound)
            {
                float pitchMixerRatio = 1.0f - ((timer - (timeUntilClosed / 3.0f)) / timeUntilClosed);
                pitchMixerRatio = Mathf.Clamp(pitchMixerRatio, 0.4f, 1.1f);
                audioSource1.pitch = pitchMixerRatio;
                audioMixer.SetFloat("MixerPitch", 1.0f / pitchMixerRatio);
            }
            if(timer <= 5.0f && !warningSoundPlayed)
            {
                audioSource2.Play();
                warningSoundPlayed = true;
            }
            if (timer <= 0)
            {
                PlaySoundEffect();
                CloseDoor();
            }
        }
    }

    public void OnFullyColoredButton()
    {
        if (open)
            return;

        OpenDoor();
    }

    public void PlaySoundEffect()
    {
        SoundEffectManager.instance.PlaySoundEffect(doorClosedSoundEffect);
    }
    private void OpenDoor()
    {
        WalkThrough(true);
        timer = timeUntilClosed;
        warningSoundPlayed = false;
        if (playSound)
            audioSource1.Play();
    }

    public void CloseDoor()
    {
        WalkThrough(false);
        button.ResetToOriginal();
        if (playSound)
        {
            audioSource1.Stop();
        }
    }

    public void WalkThrough(bool status)
    {
        openDoor.SetActive(status);
        closedDoor.SetActive(!status);
        open = status;
    }


}
