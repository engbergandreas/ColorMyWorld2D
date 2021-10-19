using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenRoom : MonoBehaviour
{
    public GameObject room;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
            room.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
            room.SetActive(false);
    }
}
