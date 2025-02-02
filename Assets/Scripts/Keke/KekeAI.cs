using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Pathfinding;
using System.Reflection;


public class KekeAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float pathUpdateRate = 0.5f;
    public float activationDistance = 50f;

    [Header("Physics")]
    public float speed = 2f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.5f;
    public float jumpForce = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behaviour")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;
    private float directionValue = 0f;


    private Pathfinding.Path path;
    private int currentWaypoint = 0;
    public bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;

    Collider2D coll;
     
    
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateRate);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
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


        int groundLayer = 6;
        int wallLayer = 7;
        int platformLayer = 8;
        int layerMask = (1<<groundLayer) | (1<<wallLayer) | (1<<platformLayer);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, coll.bounds.extents.y + jumpCheckOffset, layerMask);
        isGrounded = (hit.collider != null && rb.velocity.y <= 0);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] -rb.position).normalized;
        if (direction.x > 0)
        {
            directionValue = 1f;
            rb.velocity= new Vector2((Vector2.right * speed).x, rb.velocity.y);
        } else if (direction.x < 0)
        {
            directionValue = -1f;
            rb.velocity= new Vector2((Vector2.left * speed).x, rb.velocity.y);
        }
        // Vector2 force = direction * speed;

        if (jumpEnabled && isGrounded && HitsGround(layerMask))
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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


    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activationDistance;
    }

    private Vector2[] points = new Vector2[110];
    private Vector2 hitPoint;
    private bool HitsGround(int layerMask)
    {
        float resolution = 100f;
        float gravity = Physics2D.gravity.y * rb.gravityScale; // Effective gravity
        float timeStep = 0.05f; // Time between each step
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y + coll.bounds.extents.y);

        Vector2 prevPoint = startPos;
        points[0] = startPos;
        for (int i = 1; i <= resolution; i++)
        {
            float t = i * timeStep;

            Vector2 newPoint = startPos + new Vector2(
                speed * t * directionValue,                    // X position
                (jumpForce * t) + (0.5f * gravity * t * t) // Y position
            );
            points[i] = newPoint;
            

            RaycastHit2D hit = Physics2D.Raycast(newPoint, Vector2.down, 0.5f, layerMask);
            
            if (hit.collider != null && !(hit.point.y < hit.transform.position.y + hit.collider.bounds.extents.y))
            {
                Debug.Log("Collision detected at: " + newPoint);
                hitPoint = newPoint;
                return true; // Stop checking further if collision detected
            }
            
            prevPoint = newPoint;
            
        }
        hitPoint = Vector2.zero;
        return false;
    }

    void OnDrawGizmos()
    {
        for (int i = 1; i < points.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(points[i - 1], points[i]);
        }
        if (hitPoint != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitPoint, 0.3f);
        }
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
