using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyThrower : MonoBehaviour
{
    const float NOT_FOUND = -1.0f;

    public GameObject objTothrow;
    public Animator animator;
    public Transform firePoint;

    public float timeBetweenAttacks = 5.0f;
    public float projectileSpeed = 10.0f; //determine also how far away the target can be from origin
    private float timeSinceLastAttack = 0;
    private bool inVision = false;

    private void Start()
    {
        InvokeRepeating("VisionCheck", 1.0f, 0.1f);
    }
    /// <summary>
    /// Checks if the player is on the left side of the enemy by boxcasting
    /// </summary>
    private void VisionCheck()
    {
        inVision = Physics2D.BoxCast(firePoint.position, new Vector2(2.0f, 7.0f), 0, Vector2.left, 30.0f, LayerMask.GetMask("Player"));
    }

    /// <summary>
    /// Computes throwing angle alpha if possible using the given velocity v
    /// </summary>
    /// <returns></returns>
    private float ComputeThrowAngle()
    {
        float v = projectileSpeed;
        float g = Physics2D.gravity.y;
        Vector3 target = Player.Instance.transform.position;
        Vector3 origin = firePoint.position;
        float x = origin.x - target.x;
        float y = origin.y - target.y;

        float underroot = (v*v*v*v - g * (g * x * x + 2 * y * v * v));
        if (underroot < 0)
        {
            Debug.Log("No solution");
            return NOT_FOUND;
        }

        float sqrt = Mathf.Sqrt(underroot);
        float a1 = Mathf.Atan2(v * v + sqrt, (g * x)); //lob
        float a2 = Mathf.Atan2(v * v - sqrt, (g * x)); //direct

        //lobs if the y - value between enemy and player is more than x
        return Mathf.Abs(y) > 3.0f ? a1 : a2;
    }

    private IEnumerator ThrowObject()
    {
        float angle = ComputeThrowAngle();
        if (angle != NOT_FOUND)
        {
            animator.SetTrigger("Shoot");
            yield return new WaitForSeconds(0.5f); //hack for animation to work properly

            var obj = Instantiate(objTothrow, firePoint.position, Quaternion.identity, transform);
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            Vector2 throwDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            
            rb.velocity = throwDirection * projectileSpeed;
            Destroy(obj, 5.0f);
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(timeSinceLastAttack <= 0)
        {
            if (inVision)
            {
                StartCoroutine("ThrowObject");
                timeSinceLastAttack = timeBetweenAttacks;
            }
        }
        else
        {
            timeSinceLastAttack -= Time.deltaTime;
        }
    }
}
