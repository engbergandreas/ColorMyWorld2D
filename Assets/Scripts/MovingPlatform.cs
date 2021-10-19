using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform p1, p2;
    public float platformSpeed;
    private Rigidbody2D rb;
    private Vector3 platformVelocity;
    private Vector3 startPos, endPos, target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = p1.position;
        endPos = p2.position;
        target = startPos;
        //StartCoroutine(Vector3LerpCoroutine(startPos, platformSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target - transform.position;
        float dist = direction.magnitude;
        direction = direction.normalized;

        if (dist <= 0.1f)
            target = GetNewTarget();

        rb.velocity = direction * platformSpeed;
        
    }
    
    Vector3 GetNewTarget()
    {
        return target == startPos ? endPos : startPos;
    }
}
