using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGroundTrigger : MonoBehaviour
{
    public bool triggerActive = false;
    public bool playerInTrigger = false;
    public bool kekeInPlace = false;
    private Animator animator;

    public BoxCollider2D col;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!triggerActive) return;
        if (playerInTrigger && kekeInPlace) 
        {
            animator.SetBool("break", true);
            triggerActive = false;
            SoundManager.PlaySoundNL(SoundType.BREAKABLEBODEN, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggerActive) return;
        if (collision.CompareTag("Player")) 
        {
            MovementBaseState.LockMovement();
            this.playerInTrigger = true;
        }
    }

    public void CollisionOff() 
    {
        col.enabled = false;
        MovementBaseState.UnlockMovement();
    }

    public void SpriteOff() 
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }
}
