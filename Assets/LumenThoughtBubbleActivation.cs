using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumenThoughtBubbleActivation : MonoBehaviour
{
    LumenThoughtBubble bubble;
    [SerializeField] private Sprite buttonSprite;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            bubble = collision.transform.parent.GetChild(1).GetComponent<LumenThoughtBubble>();
            bubble.SetButtonSprite(buttonSprite);
            bubble.StartFade(1f, 2f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            bubble = collision.transform.parent.GetChild(1).GetComponent<LumenThoughtBubble>();
            bubble.StartFade(0f, 2f);
        }
    }

    public void DeactivatePrompt() 
    {
        this.GetComponent<Collider2D>().enabled = false;
        FindObjectOfType<LumenThoughtBubble>().StartFade(0f, 2f);
    }
}
