using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasselbandeStartPathing : MonoBehaviour
{
    public KekeAI pathfinder;
    public GameObject Rasselbande;
    private Rigidbody2D rb;

    public Transform target;
    private bool activated = false;

    private bool reachedBridge = false;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        pathfinder.target = target;
        pathfinder.followEnabled = true;
        activated = true;
    }
    void Start()
    {
        rb = Rasselbande.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (activated)
        {
            if (pathfinder.ReachedTarget())
            {
                pathfinder.followEnabled = false;
                reachedBridge = true;
            }
        }
        if (reachedBridge)
        {
            rb.velocity = new Vector2(2, rb.velocity.y);
            pathfinder.animator.SetFloat("xMovement", rb.velocity.x);
        }
    }
}
