using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 250.0f;
    public float jumpStrength = 5.0f;
    public Animator animator;
    public Transform groundCheck;

    public AudioClip jumpSoundEffect;
    public AudioSource audioSource;
    private CircleCollider2D col;

    private Rigidbody2D rb, platformRb;
    private bool onLadder = false, onPlatform = false;
    private int layerMask;

    //private Actions playerActions;
    private bool facingRight = true;

    Vector3 movementdir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        //playerActions = GetComponent<Actions>();

        if (!rb)
            rb = GetComponentInChildren<Rigidbody2D>();
        if (!col)
            col = GetComponentInChildren<CircleCollider2D>();

        layerMask = ~LayerMask.GetMask("Player");
    }

    public void OnEnterLadder()
    {
        onLadder = true;
        rb.gravityScale = 0;
    }
    public void OnExitLadder()
    {
        onLadder = false;
        rb.gravityScale = 1;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movementdir.x * movementSpeed * Time.deltaTime, rb.velocity.y);
        if(onLadder)
        {
            rb.velocity = new Vector2(rb.velocity.x, movementdir.y * movementSpeed * Time.deltaTime);
        }
        if(onPlatform)
        {
            rb.velocity += platformRb.velocity;
        }
    }
    //TODO: better movement controls
    void Update()
    {
        movementdir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        animator.SetFloat("Speed", Mathf.Abs(movementdir.x));
        if (absX > 0 && !animator.GetBool("IsJumping"))
        {
            if(!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }

        //flips the character in the direction it is running
        if (movementdir.x > 0 && !facingRight)
            Flip();
        else if (movementdir.x < 0 && facingRight)
            Flip();


        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            SoundEffectManager.instance.PlaySoundEffect(jumpSoundEffect);
        }
    }
    
    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag != "Player")
        {
            animator.SetBool("IsJumping", false);
        }
        if (other.transform.tag == "Platform")
        {
            platformRb = other.transform.GetComponent<Rigidbody2D>();
            onPlatform = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.transform.tag == "Platform")
        {
            onPlatform = false;
            platformRb = null;
        }
    }

    private bool IsGrounded()
    { 
        return Physics2D.OverlapCircle(groundCheck.position, col.radius * 1.1f, layerMask);
    }
}
