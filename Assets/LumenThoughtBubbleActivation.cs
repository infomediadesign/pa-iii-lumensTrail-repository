using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumenThoughtBubbleActivation : MonoBehaviour
{
    
    protected LumenThoughtBubble bubble;
    public bool showPromptNow = true;
    [SerializeField] protected BubbleEnum activationType;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!showPromptNow) return;
        if (collision.CompareTag("Player")) 
        {
            bubble = collision.transform.parent.GetChild(1).GetComponent<LumenThoughtBubble>();
            bubble.ActivateBubble(this.activationType);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!showPromptNow) return;
        if (collision.CompareTag("Player")) 
        {
            bubble = collision.transform.parent.GetChild(1).GetComponent<LumenThoughtBubble>();
            bubble.DeactivateBubble();
        }
    }
    
    public void DeactivatePrompt() 
    {
        this.GetComponent<Collider2D>().enabled = false;
        FindObjectOfType<LumenThoughtBubble>().DeactivateBubble();
    }

    public void ActivatePrompt() 
    {
        this.GetComponent<Collider2D>().enabled = true;
    }

    public void SetShowPromptNow(bool input) 
    {
        this.showPromptNow = input;
    }
}
