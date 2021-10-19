using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : DrawableObject
{
    public Transform[] patrolPoints;
    public float SearchRadius = 5.0f;
    public float speed = 5.0f;
    //private NavMeshAgent agent;

    private int currentPatrolPoint;
    private float patrolPointOffest = 0.3f;
    private Vector3 destination;
    private bool isHunting = false, idle = false, facingRight = false;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        //agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GoToNextPatrolPoint();
    }

    protected override void FullyColored()
    {
        base.FullyColored();
        //Destroy(gameObject); //TODO: enemy death animation or other thing
        transform.Translate(new Vector3(-3000, -3000, 0));
        Destroy(gameObject, 2.0f);
        
    }
    
    private void GoToNextPatrolPoint()
    {
        // Set the agent to go to the currently selected destination.
        destination = patrolPoints[currentPatrolPoint].position;
        idle = false;
        animator.SetBool("IsIdle", false);
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
    }

    private void MoveTowardsDestination()
    {
        Vector3 direction = destination - transform.position;
        transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);

        if (direction.x > 0 && !facingRight)
            Flip();
        else if (direction.x < 0 && facingRight)
            Flip();


        if (Vector3.Distance(destination, transform.position) <= patrolPointOffest)
        {
            idle = true;
            animator.SetBool("IsIdle", true);
            Invoke("GoToNextPatrolPoint", Random.Range(1.0f, 3.5f));
        }
    }

    private void Update()
    {
        if(!idle)
            MoveTowardsDestination();
        //TODO draw gizmo / visualize search radius
        //TODO kill, hurt player when close
        float playerDistance = FindPlayerDistanceSqr();
        if (playerDistance <= SearchRadius)
        {
            destination = Player.Instance.transform.position;
            isHunting = true;
        }
        else
        {
            if(isHunting)
            {
                GoToNextPatrolPoint();
                isHunting = false;
            }
        }

        
        // Choose the next destination point when the agent gets
        // close to the current one.
        //if (!agent.pathPending && agent.remainingDistance < 0.5f)
        //    GoToNextPatrolPoint();

        //rotates plane towarads camera when using NavMeshAgent
        //transform.eulerAngles = new Vector3(90, 180, -agent.transform.rotation.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            Player.Instance.PlayerHitByEnemy(Vector2.up * 3.0f);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private float FindPlayerDistanceSqr()
    {
        return Vector3.Magnitude(Player.Instance.transform.position - transform.position);
    }
}
