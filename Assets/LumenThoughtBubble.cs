using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum BubbleEnum 
{
    LMB,
    LMB_HOLD,
    RMB,
    KEY_E
}
public class LumenThoughtBubble : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 offset;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.SetAlphaZero();
        offset = transform.localPosition;
    }

    private void Update()
    {
        this.transform.position = target.transform.position + offset;   
    }

    public void ActivateBubble(BubbleEnum input) 
    {
        switch (input) 
        {
            case BubbleEnum.LMB:
                this.animator.SetBool("LMB", true);
                break;
            case BubbleEnum.LMB_HOLD:
                this.animator.SetBool("LMB_hold", true);
                break;
            case BubbleEnum.RMB:
                this.animator.SetBool("RMB", true);
                break;
            case BubbleEnum.KEY_E:
                this.animator.SetBool("Key_E", true);
                break;
            default:
                Debug.Log("Baaaaka");
                break;
        }
    }

    public void DeactivateBubble() 
    {
        this.animator.SetBool("LMB", false);
        this.animator.SetBool("LMB_hold", false);
        this.animator.SetBool("RMB", false);
        this.animator.SetBool("Key_E", false);
    }

    public void SetAlphaOne() 
    {
        Color color = spriteRenderer.color;
        color.a = 1;
        this.spriteRenderer.color = color;
    }

    public void SetAlphaZero()
    {
        Color color = spriteRenderer.color;
        color.a = 0;
        this.spriteRenderer.color = color;
    }
}
