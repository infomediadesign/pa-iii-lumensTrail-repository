using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PropellerFlowerUpstreamFlow : MonoBehaviour
{
    private PropellerFlower parent;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        parent = GetComponentInParent<PropellerFlower>();
        this.DeactivateRenderer();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (parent != null)
        {
            parent.OnPlayerStayWindTrigger(collision);
        }
    }

    public void ActivateRenderer() 
    {
        Color color = this.spriteRenderer.color;
        color.a = 1;
        this.spriteRenderer.color = color;
        this.animator.SetBool("active", true);
    }

    public void DeactivateRenderer() 
    {
        Color color = this.spriteRenderer.color;
        color.a = 0;
        this.spriteRenderer.color = color;
        this.animator.SetBool("active", false);
    }
}
