using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedLevel : MonoBehaviour
{
    public Transform teleportTo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = teleportTo.position;
    }
}
