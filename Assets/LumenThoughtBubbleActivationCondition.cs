using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumenThoughtBubbleActivationCondition : MonoBehaviour
{
    [SerializeField] private LumenThoughtBubbleActivation target;

    void Start()
    {
        target.SetShowPromptNow(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            target.SetShowPromptNow(true);
        }
    }
}
