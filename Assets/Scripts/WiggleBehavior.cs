using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleBehavior : MonoBehaviour
{
    public float degrees;
    public float speed;

    private float startangle;

    private void Start()
    {
        startangle = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * speed) * degrees + startangle );
    }
}
