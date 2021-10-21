using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenRoom : MonoBehaviour
{
    public GameObject room;
    public TilemapRenderer tmap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            tmap.enabled = false;
            room.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            tmap.enabled = true;
            room.SetActive(false);
        }
    }
}
