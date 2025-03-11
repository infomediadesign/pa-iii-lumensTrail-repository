using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Pathfinding;
using System.Reflection;
using Unity.VisualScripting;
using System;

public class KekeAIAdvanced : MonoBehaviour
{

   /*  public Animator animator;
    public enum KekeState {Following, Jumping, Falling}
    
    [Header("Designer Variables")]
    [Header("Pathfinding")]
    public Transform target;
    public float activationDistance = 50f;



    [Header("Physics")]
    public float speed = 2f;
    public float jumpForce = 0.3f;
    public float jumpUnblockTime = 0.3f;
    public int jumpFragments = 4;

    
    [Header("Programmer Variables")]
    [Header("Pathfinding")]
    
    public float pathUpdateRate = 0.5f;
    

    [Header("Physics")]
    private float jumpSpeed;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.5f;
    public float jumpCheckOffset = 0.1f;

    public float groundedCheckDistance = 0.1f;
    private float castDistance;

    public Vector2 footBoxSize;
    public float dropCheckOffset = 30f;
    private bool pathDrops=false;
    public float reachThreshold = 0.5f;


    [Header("Custom Behaviour")]
    
    public KekeState currentState = KekeState.Following;
    
    public bool followEnabled = false;

    private bool running = false;
    public bool jumpEnabled = true;
    private bool jumpBlocked = false;
    public bool directionLookEnabled = true;
    private float directionValue = 0f;

    public LayerMask collisionMask;


    private Pathfinding.Path path;
    private int currentWaypoint = 0;
    public bool isGrounded = false;
    private GameObject ground;
    private GameObject fallThrough;
    private float jumpTimer= 0;

    Seeker seeker;

    [SerializeField] public GridGraph gridGraph;

    public GameObject pathfinder;
    
    Rigidbody2D rb;

    public Collider2D coll; */


    
    private PolygonCollider2D maxJumpCollider;
    private GameObject maxJumpColliderObject;
    
    [SerializeField] private float maxJumpHeight = 5f;
    private float maxJumpStartSpeed;
    [SerializeField] private float maxJumpColliderHeightOffset = 0.5f;
    [SerializeField] private float maxFallHeight = 20f;
    [SerializeField] private int polygonColliderTopBottomPoints = 10;
    [SerializeField] private float maxSpeedX = 4f;
     
    
    // Start is called before the first frame update
    void Start()
    {
        /* seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        int groundLayer = 6;
        int wallLayer = 7;
        int platformLayer = 8;
        collisionMask = (1<<groundLayer) | (1<<wallLayer) | (1<<platformLayer);

        target = GameObject.Find("Player").transform;
        animator.SetFloat("yMovement", 0);
        animator.SetFloat("xMovement", 0);
        animator.SetBool("MovingRight", false);

        castDistance = groundedCheckDistance + coll.bounds.extents.y; */



        maxJumpColliderObject = new GameObject("MaxJumpCollider");
        maxJumpColliderObject.transform.parent = this.transform;
        maxJumpColliderObject.transform.localPosition = Vector3.zero;
        maxJumpCollider = maxJumpColliderObject.AddComponent<PolygonCollider2D>();

        maxJumpStartSpeed = Mathf.Sqrt(2 * maxJumpHeight * Mathf.Abs(Physics2D.gravity.y));
        
        Vector2[] points = GetMaxJumpColliderPoints();
        maxJumpCollider.SetPath(0, points);
        gizmoColliderPolygon = points;



        /* gridGraph = pathfinder.GetComponent<AstarPath>().graphs[0] as GridGraph;
        InvokeRepeating("UpdatePath", 0f, pathUpdateRate); */
    }

    private Vector2[] GetMaxJumpColliderPoints()
    {
        float gravity = Math.Abs(Physics2D.gravity.y);
        Vector2[] points = new Vector2[polygonColliderTopBottomPoints*2];
        float xStart = transform.position.x;
        float yStart = transform.position.y;
        float vy = Mathf.Sqrt(2 * gravity * (maxJumpHeight - maxJumpColliderHeightOffset));

        float tMax = vy / gravity;
        float yMax = maxJumpStartSpeed * tMax - 0.5f * gravity * tMax * tMax;
        float yMin = 0 - maxFallHeight;

        Tuple<float, float>[] ytValues = new Tuple<float, float>[polygonColliderTopBottomPoints];
        ytValues[0] = new Tuple<float, float>(yMax, tMax);

        for (int i = 0; i < polygonColliderTopBottomPoints-1; i++)
        {
            float y = yMin + ((polygonColliderTopBottomPoints-1)- i) * ((yMax - yMin) / (polygonColliderTopBottomPoints));
            Tuple<float, float> tYTup = MNF(-(0.5f * gravity), vy, yStart-y);
            if (tYTup == null)
            {
                throw new System.Exception("Unable to calculate tY");
            }
            float tY = 0;
            if (tYTup.Item1 > tMax)
            {
                if (tYTup.Item2 > tMax)
                {
                    if (tYTup.Item1 > tYTup.Item2)
                    {
                        tY = tYTup.Item1;
                    }
                    else 
                    {
                        tY = tYTup.Item2;
                    }
                }
                else
                {
                    tY = tYTup.Item1;
                }
            }
            else if (tYTup.Item2 > tMax)
            {
                tY = tYTup.Item2;
            }
            else 
            {
                throw new System.Exception("Unable to calculate tMin");
            }

            ytValues[i+1] = new Tuple<float, float>(y, tY);
        }

        for (int i = 0; i < polygonColliderTopBottomPoints; i++)
        {
            points[i] = new Vector2(maxSpeedX * ytValues[i].Item2, ytValues[i].Item1);
        }
        for (int i = 0; i < polygonColliderTopBottomPoints; i++)
        {
            points[i+polygonColliderTopBottomPoints] = new Vector2(-(maxSpeedX * ytValues[polygonColliderTopBottomPoints - i-1].Item2), ytValues[polygonColliderTopBottomPoints-i-1].Item1);
        }


        return points;
    }

    private Tuple<float, float> MNF(float a, float b, float c)
    {
        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            return null;
        }

        float x1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        float x2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

        return new Tuple<float, float>(x1, x2);
    }

    Vector2[] gizmoColliderPolygon;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(transform.position.x + gizmoColliderPolygon[0].x, transform.position.y + gizmoColliderPolygon[0].y), new Vector2(transform.position.x + gizmoColliderPolygon[gizmoColliderPolygon.Length - 1].x, transform.position.y + gizmoColliderPolygon[gizmoColliderPolygon.Length - 1].y));
        for (int i = 0; i < gizmoColliderPolygon.Length-1; i++)
        {
            Gizmos.DrawLine(new Vector2(gizmoColliderPolygon[i].x+transform.position.x, gizmoColliderPolygon[i].y+transform.position.y), new Vector2(gizmoColliderPolygon[i + 1].x + transform.position.x, gizmoColliderPolygon[i + 1].y + transform.position.y));
        }
    }

    // Update is called once per frame
    /*    void FixedUpdate()
        {        
            switch (currentState)
            {
                case KekeState.Following:
                    if (!followEnabled)
                    {
                        if (running && isGrounded)
                        {
                            running = false;
                            rb.velocity = Vector2.zero;
                            animator.SetFloat("xMovement", rb.velocity.x);
                            animator.SetFloat("yMovement", 0);
                        }
                        return;
                    }
                    if (TargetInDistance() && followEnabled)
                    {
                        running = true;
                        PathFollow();
                    }
                    break;
                case KekeState.Jumping:
                    ExecuteJump();
                    break;
                case KekeState.Falling:
                    ExecuteFalling();
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


            animator.SetFloat("xMovement", rb.velocity.x);
            animator.SetFloat("yMovement", 0);

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint+1] -(Vector2)path.vectorPath[currentWaypoint]).normalized;


            //RaycastHit2D cliffCheck = Physics2D.Raycast(new Vector2(transform.position.x + coll.bounds.extents.x * Mathf.Sign(direction.x), transform.position.y), Vector2.down, coll.bounds.extents.y + jumpCheckOffset, collisionMask);
            //if (cliffCheck.collider != null)
            //{
                // rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            if (direction.x > 0)
            {
                directionValue = 1f;
                animator.SetBool("MovingRight", true);
                rb.velocity= new Vector2((Vector2.right * speed).x, rb.velocity.y);
            } else if (direction.x < 0)
            {
                directionValue = -1f;
                animator.SetBool("MovingRight", false);
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
            bool jumpingNotFalling = true;
            bool falls = pathDrops && ground.layer == LayerMask.NameToLayer("Platform");

            if (jumpEnabled && isGrounded)
            {
                if (((direction.y > jumpNodeHeightRequirement && !jumpBlocked) || (isOnCliff && !jumpBlocked) || (falls&&!jumpBlocked)) && JumpAndFall(out jumpingNotFalling, jumpFragments, falls))
                {
                    if (jumpingNotFalling)
                    {
                        StartJump();
                    }
                    else
                    {
                        StartFalling();
                    }
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
            jumpTimer = 0;
            int kekelayer = LayerMask.NameToLayer("Keke");
            int platformLayer = LayerMask.NameToLayer("Platform");

            Physics2D.IgnoreLayerCollision(kekelayer, platformLayer, true);
            collisionResolved = false;
            animator.SetTrigger("jump");
            fallTriggered = false;
        } 

        private IEnumerator JumpUnblock(float jumpUnblockTime)
        {
            yield return new WaitForSeconds(jumpUnblockTime);
            jumpBlocked = false;
        }


        bool collisionResolved = false;
        bool fallTriggered = false;
        private void ExecuteJump()
        {

            animator.SetFloat("yMovement", rb.velocity.y);
            jumpTimer = jumpTimer + Time.deltaTime;
            if (!fallTriggered && rb.velocity.y < 0)
            {
                fallTriggered = true;
                animator.SetTrigger("fall");
            }
            if (jumpTimer < jumpTime/2) return;
            if (!collisionResolved)
            {
                collisionResolved = true;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Keke"), LayerMask.NameToLayer("Platform"), false);
            }


            CheckGrounded();


            if (isGrounded)
            {
                StartCoroutine(JumpUnblock(jumpUnblockTime));
                currentState = KekeState.Following;
                animator.SetTrigger("landing");
            }


        }

        private void StartFalling()
        {
            animator.SetTrigger("fall");
            fallThrough = ground;
            Physics2D.IgnoreCollision(coll, fallThrough.GetComponent<Collider2D>(), true);
            jumpBlocked = true;
            currentState = KekeState.Falling;
            if (directionValue > 0)
            {
                rb.velocity= new Vector2((Vector2.right * jumpSpeed).x, rb.velocity.y);
            } else if (directionValue < 0)
            {
                rb.velocity= new Vector2((Vector2.left * jumpSpeed).x, rb.velocity.y);
            }
            jumpTimer = 0;

        }

        private void ExecuteFalling()
        {        


            jumpTimer = jumpTimer + Time.deltaTime;
            if (jumpTimer < jumpTime/2) return;


            CheckGrounded();
            if (isGrounded)
            {
                StartCoroutine(JumpUnblock(jumpUnblockTime));
                currentState = KekeState.Following;
                Physics2D.IgnoreCollision(coll, fallThrough.GetComponent<Collider2D>(), false);
                fallThrough = null;
                animator.SetTrigger("landing");
            }
        }

        private void CheckGrounded()
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position - transform.up * castDistance, footBoxSize, 0, Vector2.down, groundedCheckDistance, collisionMask);
            isGrounded = (hit.collider != null && rb.velocity.y <= 0);
            if (isGrounded)
            {
                ground = hit.collider.gameObject;
            }
            else
            {
                ground = null;
            }
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

        public float jumpOffsetBuffer = 2f;
        private float jumpTime;


        private bool JumpAndFall(out bool jumpingNotFalling, int fragment = 1, bool shouldFall = false)
        {
            bool hitsGround = false;
            jumpingNotFalling = true;
            hitPoint = Vector2.zero;
            hitsGround = HitsGround(jumpForce, fragment);
            if (shouldFall)
            {
                Vector2 jumpPoint = hitPoint;
                if (HitsGround(0, 2, true)) hitsGround = true;
                if (jumpPoint != hitPoint)
                {
                    jumpingNotFalling = false;
                }
            }
            return hitsGround;
        }   

        private Vector2 jumpGoal;

        private bool HitsGround(float givenForce, int fragments = 1, bool jumping = false)
        {
            float gravity = Physics2D.gravity.y * rb.gravityScale; // Effective gravity
            float timeStep = 0.01f; // Time between each step
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y + coll.bounds.extents.y);
            bool firstHit = false;
            Vector2 jumpPoint = CalculateJumpPoint();
            Vector2 offsetWaypoint = GetWayppointAtJumpPoint(jumpPoint);
            jumpGoal = offsetWaypoint;

            for (int dpth = fragments; dpth >= 1; dpth--)
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
                        (givenForce * t) + (0.5f * gravity * t * t) // Y position
                    );
                    tempPoints[i] = newPoint;


                    RaycastHit2D hit = Physics2D.Raycast(newPoint, Vector2.down, 0.5f, collisionMask);
                    if (hit.collider != null)
                    {
                        if (jumping && hit.collider.gameObject == ground) continue;

                        if (givenForce + (gravity * t)<0)
                        {
                            Debug.Log("Collision detected at: " + newPoint);
                            if ((hitPoint == Vector2.zero) || (firstHit && GetWayPointOffset(newPoint, offsetWaypoint) < GetWayPointOffset(hitPoint, offsetWaypoint)))
                            {
                                hitPoint = newPoint;
                                firstHit = true;
                                jumpSpeed = fragment * speed;
                                points = tempPoints;
                                jumpTime = t;
                            }

                            break; // Stop checking further if collision detected for this fragment
                        }
                    }
                    points = tempPoints;

                    prevPoint = newPoint;

                }
                points = tempPoints;

            }

            return firstHit;
        }

        private float GetWayPointOffset(Vector2 landingPoint, Vector2 waypoint)
        {
            return Vector2.Distance(waypoint, landingPoint);
        }

        List<Vector2> calculatedPoints = new List<Vector2>();
        private Vector2 GetWayppointAtJumpPoint(Vector2 jumpPoint)
        {
            calculatedPoints.Clear();
            if (Mathf.Abs(target.position.x - transform.position.x) <= (MathF.Abs(transform.position.x - jumpPoint.x)*2)) return target.position;

            for (int i = 2; i < path.vectorPath.Count; i++)
            {
                if ((int)path.vectorPath.Count/i == 0) break;
                Vector2 waypoint = (Vector2)path.vectorPath[(int)path.vectorPath.Count/i];
                calculatedPoints.Add(waypoint);
                if (Mathf.Abs(waypoint.x - transform.position.x) <= (Mathf.Abs(transform.position.x - jumpPoint.x)*2)) return waypoint;
            }
            return target.position;

            /* float jumpDistance = Mathf.Abs(transform.position.x - jumpPoint.x);
            float waypointDistance = Mathf.Abs(transform.position.x - ((Vector2)path.vectorPath[currentWaypoint]).x);
            int wayPointIndex = (int)(jumpDistance / waypointDistance)-1;
            Vector2 waypoint;


            if (wayPointIndex >= path.vectorPath.Count)
            {
                waypoint = (Vector2)path.vectorPath[path.vectorPath.Count - 1];
            }
            else
            {
                waypoint = (Vector2)path.vectorPath[wayPointIndex];
            }
            return waypoint; */

    /*

        }

        private Vector2 jumpCalculation;
        private Vector2 CalculateJumpPoint()
        {
            float gravity = Physics2D.gravity.y * rb.gravityScale;
            float timeOfFlight = (-2 * jumpForce) / gravity;
            float xLand = transform.position.x + speed * timeOfFlight;
            Vector2 landingPosition = new Vector2(xLand, transform.position.y);
            jumpCalculation = landingPosition;
            return landingPosition;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            foreach (Vector2 point in calculatedPoints)
            {
                Gizmos.DrawSphere(point, 0.3f);
            }
            //draw transform
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.3f);
            Gizmos.DrawSphere(jumpGoal, 0.3f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(jumpCalculation, 0.3f);


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

            Gizmos.DrawWireCube(transform.position - transform.up * castDistance, footBoxSize);


        }

        public bool ReachedTarget()
        {
            return Vector2.Distance(transform.position, target.position) < reachThreshold;
        }

        private void OnPathComplete(Pathfinding.Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        } */
}
