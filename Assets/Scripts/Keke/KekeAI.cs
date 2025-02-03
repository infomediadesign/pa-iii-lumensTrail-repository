using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Pathfinding;
using System.Reflection;

public class KekeAI : MonoBehaviour
{
    [Header("Designer Variables")]
    [Header("Pathfinding")]
    public Transform target;
    public float activationDistance = 50f;



    [Header("Physics")]
    public float speed = 2f;
    public float jumpForce = 0.3f;
    public float jumpUnblockTime = 0.3f;
    public int jumpFragments = 4;



    public enum KekeState {Following, Jumping}
    
    [Header("Programmer Variables")]
    [Header("Pathfinding")]
    
    public float pathUpdateRate = 0.5f;
    

    [Header("Physics")]
    private float jumpSpeed;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.5f;
    public float jumpCheckOffset = 0.1f;
    public float dropCheckOffset = 30f;
    private bool pathDrops=false;

    

    [Header("Custom Behaviour")]
    
    public KekeState currentState = KekeState.Following;
    
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    private bool jumpBlocked = false;
    public bool directionLookEnabled = true;
    private float directionValue = 0f;

    public LayerMask collisionMask;


    private Pathfinding.Path path;
    private int currentWaypoint = 0;
    public bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;

    public Collider2D coll;
     
    
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        int groundLayer = 6;
        int wallLayer = 7;
        int platformLayer = 8;
        collisionMask = (1<<groundLayer) | (1<<wallLayer) | (1<<platformLayer);

        InvokeRepeating("UpdatePath", 0f, pathUpdateRate);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
            case KekeState.Following:
                if (TargetInDistance() && followEnabled)
                {
                    PathFollow();
                }
                break;
            case KekeState.Jumping:
                ExecuteJump();
                break;
        }
        
        
    }

    private void UpdatePath()
    {
        
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        

        CheckGrounded();

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] -rb.position).normalized;

        //RaycastHit2D cliffCheck = Physics2D.Raycast(new Vector2(transform.position.x + coll.bounds.extents.x * Mathf.Sign(direction.x), transform.position.y), Vector2.down, coll.bounds.extents.y + jumpCheckOffset, collisionMask);
        //if (cliffCheck.collider != null)
        //{
            // rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        if (direction.x > 0)
        {
            directionValue = 1f;
            rb.velocity= new Vector2((Vector2.right * speed).x, rb.velocity.y);
        } else if (direction.x < 0)
        {
            directionValue = -1f;
            rb.velocity= new Vector2((Vector2.left * speed).x, rb.velocity.y);
        }


        bool isOnCliff = false;
        RaycastHit2D cliffCheck = Physics2D.Raycast(new Vector2(transform.position.x + coll.bounds.extents.x * directionValue, transform.position.y), Vector2.down, coll.bounds.extents.y + jumpCheckOffset, collisionMask);
        if (cliffCheck.collider == null)
        { 
            // RaycastHit2D dropCheck = Physics2D.Raycast(new Vector2(transform.position.x + coll.bounds.extents.x * directionValue, transform.position.y), Vector2.down, coll.bounds.extents.y + dropCheckOffset, collisionMask);
            

            // if (!(pathDrops && dropCheck.collider != null))
            // {
                rb.velocity = new Vector2(0, rb.velocity.y);
                isOnCliff = true;
            // }
        }
        if (path.vectorPath[currentWaypoint+2] != null)
        {
            pathDrops = ((Vector2)path.vectorPath[currentWaypoint+2]).y < transform.position.y;
        } else {
            pathDrops = false;
        }


       // }
       // else
       // {
            // rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
      // }
        
        // Vector2 force = direction * speed;

        if (jumpEnabled && isGrounded && HitsGround(jumpFragments))
        {
            if ((direction.y > jumpNodeHeightRequirement && !jumpBlocked) || (isOnCliff && !jumpBlocked))
            {
                StartJump();
            }
        }
        // //movement
        // rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void StartJump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpBlocked = true;
        currentState = KekeState.Jumping;
        if (directionValue > 0)
        {
            rb.velocity= new Vector2((Vector2.right * jumpSpeed).x, rb.velocity.y);
        } else if (directionValue < 0)
        {
            rb.velocity= new Vector2((Vector2.left * jumpSpeed).x, rb.velocity.y);
        }

    }

    private IEnumerator JumpUnblock(float jumpUnblockTime)
    {
        yield return new WaitForSeconds(jumpUnblockTime);
        jumpBlocked = false;
    }
    
    private void ExecuteJump()
    {
        CheckGrounded();
        

        if (isGrounded)
        {
            StartCoroutine(JumpUnblock(jumpUnblockTime));
            currentState = KekeState.Following;
        }
        
        
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, coll.bounds.size, 0, Vector2.down, jumpCheckOffset, collisionMask);
        isGrounded = (hit.collider != null && rb.velocity.y <= 0);
        
    }

    

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activationDistance;
    }

    [Header("Jumping")]
    [Min(0)]
    public const int jumpSimulationDepth = 300;
    private Vector2[] points = new Vector2[(int)jumpSimulationDepth+10];
    private Vector2 hitPoint;
    
    private bool HitsGround(int fragments = 1)
    {
        float gravity = Physics2D.gravity.y * rb.gravityScale; // Effective gravity
        float timeStep = 0.01f; // Time between each step
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y + coll.bounds.extents.y);
        bool firstHit = false;
        hitPoint = Vector2.zero;

        for (int dpth = fragments; dpth > 0; dpth--)
        {
            Vector2 prevPoint = startPos;
            points[0] = startPos;
            Vector2[] tempPoints = new Vector2[(int)jumpSimulationDepth+10];
            for (int i = 1; i <= jumpSimulationDepth; i++)
            {
                float t = i * timeStep;

                float fragment = dpth / (float)fragments;
                Vector2 newPoint = startPos + new Vector2(
                    speed * t * directionValue * fragment,                    // X position
                    (jumpForce * t) + (0.5f * gravity * t * t) // Y position
                );
                tempPoints[i] = newPoint;
                

                RaycastHit2D hit = Physics2D.Raycast(newPoint, Vector2.down, 0.5f, collisionMask);
                
                if (hit.collider != null && !(hit.point.y < hit.transform.position.y + hit.collider.bounds.extents.y) && jumpForce + (gravity * t)<0)
                {
                    Debug.Log("Collision detected at: " + newPoint);
                    if ((!firstHit) || (firstHit && Vector2.Distance(target.transform.position, newPoint) < Vector2.Distance(target.transform.position, hitPoint)))
                    {
                        hitPoint = newPoint;
                        firstHit = true;
                        jumpSpeed = fragment * speed;
                        points = tempPoints;
                    }
                    
                    break; // Stop checking further if collision detected for this fragment
                }
                
                prevPoint = newPoint;
                
            }
            
        }
        
        return firstHit;
    }

    void OnDrawGizmos()
    {
        //draw Jump
        for (int i = 1; i < points.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(points[i - 1], points[i]);
            Gizmos.DrawSphere(points[i], 0.1f);
        }
        if (hitPoint != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitPoint, 0.3f);
        }

        //draw collisionCkeck Platform
        Vector2 pointA = new Vector2(this.transform.position.x + this.coll.bounds.extents.x * directionValue, transform.position.y);
        Vector2 pointB = new Vector2(this.transform.position.x + this.coll.bounds.extents.x * directionValue, transform.position.y - (coll.bounds.extents.y + jumpCheckOffset));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointA, pointB);

         Vector2 pointC = new Vector2(this.transform.position.x + this.coll.bounds.extents.x * directionValue, transform.position.y - (coll.bounds.extents.y + jumpCheckOffset));
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(pointA, pointC);


    }
    

    private void OnPathComplete(Pathfinding.Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
