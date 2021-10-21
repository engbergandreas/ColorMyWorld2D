using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        Debug.Log("Update spawnpoint");
    }

    public void PlayerHitByEnemy(Vector2 force)
    {
        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Disappear");
        Invoke("Kill", 3.0f);

    }

    public void PlayerHitByObject()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4.0f), ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Disappear");
        Invoke("Kill", 3.0f);
    }

    public void Kill()
    {
        transform.position = respawnPoint;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Animator>().SetTrigger("Appear");
        Debug.Log("TODO: Remove life");
    }
}
