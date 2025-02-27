using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGroundTrigger : MonoBehaviour
{
    public bool triggerActive = false;
    private Animator animator;

    public BoxCollider2D col;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggerActive) return;
        if (collision.CompareTag("Player")) 
        {
            Debug.Log("Hallo");
            animator.SetBool("break", true);
        }
    }

    public void CollisionOff() 
    {
        col.enabled = false;
    }

    public void SpriteOff() 
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }
}
