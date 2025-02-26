using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KekeReceiver : CollectableReceiver
{
    public Animator kekeAnimator;
    public Transform chaseStartPosition;
    
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

    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        kekeAnimator.SetTrigger("eating");
    }
}
