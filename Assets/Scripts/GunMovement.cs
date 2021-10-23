using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMovement : MonoBehaviour
{
    public Transform target;
    public GameObject gun;
    public Transform sprite;
    private Camera _cam;
    public float radie = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        _cam = FindObjectOfType<Camera>();
        Player.Instance.onHitEvent.AddListener(OnPlayerDeath);
        Player.Instance.deathEvent.AddListener(OnPlayerSpawn);
    }

    void OnPlayerDeath()
    {
        gun.SetActive(false);
    }
    void OnPlayerSpawn()
    {
        gun.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 dir = _cam.ScreenToWorldPoint(Input.mousePosition) - target.position;
        //dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        float x = Mathf.Cos(angle) * radie;
        float y = Mathf.Sin(angle) * radie;

        gun.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);
        gun.transform.position = new Vector3(x, y, 0) + target.position;

        if (dir.x < 0)
            sprite.localEulerAngles = new Vector3(180, 0, 0);
        else
            sprite.localEulerAngles = Vector3.zero;

    }
}
