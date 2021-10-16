using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnsafeGround : MonoBehaviour
{
    /// <summary>
    /// Given in seconds 
    /// </summary>
    [Range(0.5f, 2.0f)]
    public float lifeTime;
    //[Range(1.0f, 3.0f)]
    //public float reductionScalar = 1.0f;

    public GameObject platform;
    
    private Vector3 ogPosition, ogScale;
    private Quaternion ogRotation;
    private float resetTime = 5.0f;
    private Animator animator;
    private bool falling;
    //public float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        ogPosition = platform.transform.position;
        ogRotation = platform.transform.rotation;
        ogScale = platform.transform.localScale;
        animator = platform.GetComponent<Animator>();
        Reset();
    }
    private void Update()
    {
        if (falling)
            Fall();

    }
    IEnumerator ShakeGround()
    {
        for (int i = 0; i < lifeTime * 10; i++)
        {
            platform.transform.Translate(new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.05f, 0.05f)), Space.World);
            yield return new WaitForSeconds(0.1f) ;
        }

        falling = true;
        animator.SetBool("IsOn", false);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void Reset()
    {
        platform.transform.position = ogPosition;
        platform.transform.rotation = ogRotation;
        platform.transform.localScale = ogScale;
        //timeLeft = lifeTime;
        animator.SetBool("IsOn", true);
        falling = false;
        GetComponent<BoxCollider2D>().enabled = true;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 direction = collision.transform.position - transform.position;
        if(direction.y > 0) //We hit object from the top
        {
            StartCoroutine("ShakeGround");
        }
    }

    
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    timeLeft -= Time.deltaTime * reductionScalar;
    //    if (timeLeft <= 0)
    //    {


    //    }
    //    else if(timeLeft <= lifeTime * (1/3.0f))
    //    {
    //        ShakeGround();
    //    }
    //}

    void Fall()
    {
        platform.transform.Translate(Vector3.down * 3 * Time.deltaTime);

        if (Vector3.Distance(ogPosition, platform.transform.position) > 10)
            Reset();
    }

    void OnGroundFallen()
    {
        Invoke("Reset", resetTime);
    }
}
