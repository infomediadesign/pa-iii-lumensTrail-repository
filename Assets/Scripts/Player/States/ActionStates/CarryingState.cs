using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryingState : ActionBaseState
{
    private GameObject carriedObject;
    private Rigidbody2D carriedRb;
    private Transform transform;
    private float speedMod;
    private float carryHeight;
    
    public CarryingState(StateMachine sm):base (sm)
    {
        ownState = ActionBaseState.StateKey.Carrying;
    }


    public override void SwitchTo()
    {
        if ((ActionBaseState.StateKey)sm.currentActionState.ownState != ActionBaseState.StateKey.PickUp) return;
        base.SwitchTo();
    }

    public override void OnEnter()
    {
        sm.animator.SetBool("isCarrying", true);
        carriedObject = sm.im.carriedItem;
        carryHeight = Mathf.Abs(carriedObject.transform.position.y - sm.rb.transform.position.y);
        carriedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        transform = sm.transform;
        carriedRb = carriedObject.GetComponent<Rigidbody2D>();
        
        //to be changed to design SO
        speedMod = sm.dData.speedModWhileCarrying;
        MovementBaseState.movementSpeedModifier *= speedMod;
    }

    public override void OnUpdate()
    {
       carriedObject.transform.position = new Vector2(transform.position.x, transform.position.y + carryHeight);
    }

    /**
     * @outdated: uses speedModifier instead
     **/
    
    /*public override void OnMove()
    {
        float targetSpeed = sm.horizontalMovement * sm.dData.moveSpeed * speedMod;
        sm.rb.velocity = (Vector2.right * targetSpeed + Vector2.up * sm.rb.velocity);
        carriedRb.velocity = (Vector2.right * targetSpeed + Vector2.up * sm.rb.velocity);
    }*/

    public override void OnExit()
    {
        carriedObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        sm.im.carriedItem = null;
        MovementBaseState.movementSpeedModifier /= speedMod;
        sm.animator.SetBool("isCarrying", false);
    }
}
