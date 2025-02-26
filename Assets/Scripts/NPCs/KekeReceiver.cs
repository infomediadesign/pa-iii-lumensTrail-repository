using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KekeReceiver : CollectableReceiver
{
    public Animator kekeAnimator;
    public ChaseExecution chaseExecution;

    public ItemManager itemManager;
    
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
        chaseExecution.enabled = true;
    }
}
