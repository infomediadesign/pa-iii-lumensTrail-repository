using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RasselbandeStartWalking : MonoBehaviour
{
    [SerializeField] DesignerPlayerScriptableObject dData;
    private bool isWalking = false;
    private bool isJumping = false;
    private bool statueDown = false;
    private Rigidbody2D rb;
    private Animator animator;
    private float movingSpeed = 1.5f;
    private float jumpForceMultiplier = 5f;

    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.movingSpeed = dData.rasselbandeMovingSpeed;
        this.jumpForceMultiplier = dData.rasselbandeJumpForceMultiply;
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
        this.transform.position = new Vector2(transform.position.x, transform.position.y + 0.06f);
    }

    public void SetStatueDownTrue() 
    {
        this.statueDown = true;
        Debug.Log(this.transform.childCount);
        if (this.transform.childCount > 0) this.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void JumpNow()
    {
        rb.AddForce(Vector2.up * jumpForceMultiplier, ForceMode2D.Impulse);
        this.isJumping = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isJumping) return;
        if (collision.gameObject.GetComponent<StatueMainBody>() != null)
        {
            this.isJumping = false;
            this.animator.SetTrigger("fall");
            this.transform.position = new Vector2(transform.position.x, transform.position.y + 0.04f);
        }
    }

    void OnBecameVisible()
    {
        if (statueDown)
        {
            this.isWalking = true;
            this.animator.SetTrigger("isWalking");
        }
    }
}
