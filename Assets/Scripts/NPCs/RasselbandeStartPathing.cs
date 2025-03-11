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

    public bool reachedBridge = false;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        pathfinder.target = target;
        pathfinder.followEnabled = true;
        activated = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;   
    }
    void Start()
    {
        rb = Rasselbande.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (reachedBridge)
        {
            rb.velocity = new Vector2(2, rb.velocity.y);
            pathfinder.animator.SetFloat("xMovement", rb.velocity.x);
        }
    }
}
