using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedState : PhysicsBaseState
{

    public GroundedState(StateMachine p_sm) : base(p_sm) 
    { 
        ownState = PhysicsBaseState.StateKey.Grounded;
    }

    public override void SwitchTo()
    {
        base.SwitchTo();
    }


    public override void OnEnter()
    {
        MovementBaseState.movementSpeedModifier *= sm.dData.frictionAmount;
    }

    public override void OnUpdate()
    {
        /**
         * @outdated: now uses MovementSpeedModifier
         **/
        
        /*// friction while on ground
        if (sm.pData.isGrounded && sm.rb.velocity.x < sm.dData.moveSpeed)
        {
            float amount = Mathf.Min(Mathf.Abs(sm.rb.velocity.x), Mathf.Abs(sm.dData.frictionAmount));
            amount *= Mathf.Sign(sm.rb.velocity.x);
            sm.rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }*/

        // change to airborne (falling) in case of suddenly ungrounded, but has coyoteTime
        if (sm.pData.groundCoyoteTimeCounter < 0)
        {
            sm.SwitchToState(PhysicsBaseState.StateKey.Airborne);
        }
    }

    public override void OnExit()
    {

        MovementBaseState.movementSpeedModifier /= sm.dData.frictionAmount;
    } 
}
