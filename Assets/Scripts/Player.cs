using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Different channels player & objects can be in, found in player.cs
/// </summary>
public enum RGBChannel
{
    Red, Green, Blue
};

public class Player : MonoBehaviour
{
    public static Player Instance;
    public RGBChannel playerChannel;
    public AudioClip respawnSoundEffect, deathSoundEffect, checkPointReachedSoundEffect;


    public UnityEvent onHitEvent,deathEvent;
    private Vector3 respawnPoint;

    void Awake()
    {
        Instance = this;
        respawnPoint = GameObject.Find("SpawnPoint").transform.position;
        transform.position = respawnPoint;
    }

    public void SetSpawnPoint(Vector3 point)
    {
        respawnPoint = point;
        SoundEffectManager.instance.PlaySoundEffect(checkPointReachedSoundEffect, 0.2f);
        Debug.Log("Update spawnpoint");
    }

    public void PlayerHitByEnemy(Vector2 force)
    {
        onHitEvent.Invoke();
        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Disappear");
        GetComponent<Animator>().SetBool("IsDead", true);
        SoundEffectManager.instance.PlaySoundEffect(deathSoundEffect);
        Invoke("Kill", 3.0f);
    }

    public void PlayerHitByObject()
    {
        onHitEvent.Invoke();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4.0f), ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Disappear");
        GetComponent<Animator>().SetBool("IsDead", true);
        SoundEffectManager.instance.PlaySoundEffect(deathSoundEffect);
        Invoke("Kill", 3.0f);
    }

    public void Kill()
    {
        deathEvent.Invoke();
        transform.position = respawnPoint;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Animator>().SetBool("IsDead", false);
        GetComponent<Animator>().SetTrigger("Appear");
        SoundEffectManager.instance.PlaySoundEffect(respawnSoundEffect);
        Debug.Log("TODO: Remove life");
    }
}
