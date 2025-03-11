using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCBubbleEnum 
{
    FRUIT,
    BRIDGE
}

public class ThoughtBubble : MonoBehaviour
{
    private CollectableReceiver parent;
    private SpriteRenderer spriteRenderer;
    private bool itemDeliveredFade = false;

    [SerializeField] private NPCBubbleEnum activationType;
    [SerializeField] private DesignerPlayerScriptableObject dData;
    [SerializeField] private ProgrammerPlayerScriptableObject pData;

    private Animator animator;


    void Awake()
    {
        parent = GetComponentInParent<CollectableReceiver>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.SetAlphaZero();
        this.animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void ActivateBubble(NPCBubbleEnum input) 
    {
        switch (input) 
        {
            case NPCBubbleEnum.FRUIT:
                this.animator.SetBool("Fruit", true);
                break;
            case NPCBubbleEnum.BRIDGE:
                this.animator.SetBool("Bridge", true);
                break;
            default:
                Debug.Log("Baaaaka");
                break;
        }
    }

    public void DeactivateBubble() 
    {
        this.animator.SetBool("Fruit", false);
        this.animator.SetBool("Bridge", false);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!itemDeliveredFade) this.ActivateBubble(this.activationType);
            pData.inDropRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!itemDeliveredFade) this.DeactivateBubble();
            pData.inDropRange = false;
        } 
    }

    public void SetItemDeliveredFadeTrue()
    {
        this.itemDeliveredFade = true;
        this.DeactivateBubble();
        StartCoroutine(ItemDeliveredFadeWait());
    }

    private IEnumerator ItemDeliveredFadeWait()
    {
        yield return new WaitForSeconds(dData.itemDeliveredFateTime);
        if (parent.GetDeliveredItems() == parent.GetTotalItems()) this.gameObject.SetActive(false); 
        itemDeliveredFade = false;
        if (pData.inDropRange) this.ActivateBubble(this.activationType);
    }
}
