using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyThrower : MonoBehaviour
{
    const float NOT_FOUND = -1.0f;

    public GameObject objTothrow;
    public float timeBetweenAttacks = 5.0f;
    public float projectileSpeed = 10.0f;
    private float timeSinceLastAttack = 0;
    
    private float ComputeThrowAngle()
    {
        float v = projectileSpeed;
        float g = Physics2D.gravity.y;
        Vector3 target = Player.Instance.transform.position;
        Vector3 origin = transform.position;
        float x = origin.x - target.x;
        float y = origin.y - target.y;

        float underroot = (v*v*v*v - g * (g * x * x + 2 * y * v * v));
        if (underroot < 0)
        {
            Debug.Log("No solution");
            return NOT_FOUND;
        }

        float sqrt = Mathf.Sqrt(underroot);
        float a1 = Mathf.Atan2(v * v + sqrt, (g * x));
        float a2 = Mathf.Atan2(v * v - sqrt, (g * x));

        return Mathf.Abs(y) > 1.0 ? a1 : a2;
    }

    private void ThrowObject()
    {
        float angle = ComputeThrowAngle();
        if (angle == NOT_FOUND)
            return;

        var obj = Instantiate(objTothrow, transform.position, Quaternion.identity, transform);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        Vector2 throwDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));


        rb.velocity = throwDirection * projectileSpeed;

        Destroy(obj, 5.0f);

    }
    // Update is called once per frame
    void Update()
    {
        if(timeSinceLastAttack <= 0)
        {
            ThrowObject();
            timeSinceLastAttack = timeBetweenAttacks;
        }else
        {
            timeSinceLastAttack -= Time.deltaTime;
        }
    }
}
