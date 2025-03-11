using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KekeReceiver : CollectableReceiver
{
    public Animator kekeAnimator;
    public GameObject chaseExecution;

    public ItemManager itemManager;

    public LumenThoughtBubbleActivation lumenThoughtBubbleActivation;
    
    protected override void Awake()
    {
        base.Awake();
    }

    private new void Update()
    {
        base.Update();
    }

    protected override void ItemsDeliveredTrigger()
    {
        kekeAnimator.SetTrigger("standup");
        itemManager.items = null;
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        kekeAnimator.SetTrigger("eating");
    }

    public void OnStanding()
    {
        chaseExecution.SetActive(true);
    }
}
