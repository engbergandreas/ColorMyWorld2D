using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharger : MonoBehaviour
{
    public float chargeUpTime = 2.5f;
    public float force = 2;

    public Animator animator;

    private Rigidbody2D rb;    

    public float timer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = chargeUpTime;
    }

    private void Update()
    {
        if (timer <= 0)
        {
            rb.AddForce(transform.right * force, ForceMode2D.Impulse);
            timer = chargeUpTime;
            transform.Rotate(Vector3.up, 180, Space.Self);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            Player.Instance.PlayerHitByEnemy(Vector2.up * 3.5f);
        }

        Vector2 vel = collision.relativeVelocity;
        if(vel.x < 0)
        {
            animator.SetTrigger("RightCollision");
        }
        else if(vel.x > 0 )
        {
            animator.SetTrigger("LeftCollision");
        }
    }
}
