using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnsafeGround : MonoBehaviour
{
    public float lifeTime;
    [Range(1.0f, 3.0f)]
    public float reductionScalar = 1.0f;
    
    private Vector3 ogPosition, ogScale;
    private Quaternion ogRotation;
    private float resetTime = 5.0f;

    public float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        ogPosition = transform.position;
        ogRotation = transform.rotation;
        ogScale = transform.localScale;
        Reset();
    }

    private void Reset()
    {
        transform.position = ogPosition;
        transform.rotation = ogRotation;
        transform.localScale = ogScale;
        timeLeft = lifeTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        timeLeft -= Time.deltaTime * reductionScalar;
        if (timeLeft <= 0)
        {
            Fall();
        }
    }

    void Fall()
    {
        transform.Translate(Vector3.down * 50);
        OnGroundFallen();
    }

    void OnGroundFallen()
    {
        Invoke("Reset", resetTime);
    }
}
