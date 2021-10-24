using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedLevel : MonoBehaviour
{
    public Transform teleportTo;
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.root.position = teleportTo.position + Vector3.up * 4;
        audioSource.clip = audioClip;
        audioSource.Play();
        //collision.transform.position = teleportTo.position;
    }
}
