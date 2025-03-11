using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonsaiLumenThoughtBubble : LumenThoughtBubbleActivation
{
    private bool isActive = false;
    private StateMachine sm;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!showPromptNow) return;
        if (collision.CompareTag("Player")) 
        {
            sm = collision.transform.GetComponent<StateMachine>();
            if (sm.currentActionState.ownState.Equals(ActionBaseState.StateKey.Carrying))
            {
                bubble = collision.transform.parent.GetChild(1).GetComponent<LumenThoughtBubble>();
                bubble.ActivateBubble(this.activationType);
                isActive = true;
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (isActive && !sm.currentActionState.ownState.Equals(ActionBaseState.StateKey.Carrying))
        {
            this.gameObject.SetActive(false);
        }
        
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!showPromptNow) return;
        if (collision.CompareTag("Player")) 
        {
            bubble = collision.transform.parent.GetChild(1).GetComponent<LumenThoughtBubble>();
            bubble.DeactivateBubble();
            isActive = false;
        }
    }
}
