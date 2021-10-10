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
    }

    public void SetSpawnPoint(Vector3 point)
    {
        respawnPoint = point;
    }

    public void Kill()
    {
        transform.position = respawnPoint;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Debug.Log("TODO: Remove life");
    }
}
