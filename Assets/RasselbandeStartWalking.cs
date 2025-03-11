using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasselbandeStartWalking : MonoBehaviour
{
    private bool isWalking = false;
    private Rigidbody2D rb;
    private Animator animator;
    private float movingSpeed = 1f;
    private float jumpForceMultiplier = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isWalking) 
        {
            rb.velocity = Vector2.right * movingSpeed + Vector2.up * rb.velocity;
        }
    }

    public void OnJump()
    {
        this.animator.SetTrigger("jump");
        rb.AddForce(Vector2.up * jumpForceMultiplier, ForceMode2D.Impulse);
    }

    public void SetIsWalkingTrue() 
    {
        this.isWalking = true;
        this.animator.SetTrigger("isWalking");
    }
}
