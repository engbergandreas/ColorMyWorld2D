using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class DoorButton : MonoBehaviour
{

    public float timeUntilClosed = 5;

    public GameObject closedDoor, openDoor;

    public DrawableObject button;

    private AudioSource audioSource;
    public AudioMixer audioMixer;
    
    private bool open = false;
    private float timer = 0;

    private void Start()
    {
        button._event.AddListener(OnFullyColoredButton);
        audioSource = GetComponent<AudioSource>();
        CloseDoor();
    }

    private void Update()
    {
        if (open)
        {
            timer -= Time.deltaTime;
            float pitchMixerRatio = 1.0f - ((timer - (timeUntilClosed / 3.0f)) / timeUntilClosed);
            pitchMixerRatio = Mathf.Clamp(pitchMixerRatio, 0.4f, 1.1f);
            audioSource.pitch = pitchMixerRatio;
            audioMixer.SetFloat("MixerPitch", 1.0f / pitchMixerRatio);

            if (timer <= 0)
            {
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

    private void OpenDoor()
    {
        WalkThrough(true);
        timer = timeUntilClosed;
        audioSource.Play();
    }
    
    public void CloseDoor()
    {
        WalkThrough(false);
        button.ResetToOriginal();
        audioSource.Stop();
    }

    public void WalkThrough(bool status)
    {
        openDoor.SetActive(status);
        closedDoor.SetActive(!status);
        open = status;
    }


}
