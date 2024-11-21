using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedState : BaseState
{
    public GroundedState(StateMachine p_sm) : base(p_sm) 
    { 
        stateKey = StateMachine.StateKey.Grounded;
    }

    public override void SwitchTo()
    {
        base.SwitchTo();
    }

    protected override void OnEnter()
    {

    }

    protected override void OnUpdate()
    {
        // friction while on ground
        if (sm.pData.isGrounded && sm.rb.velocity.x < sm.dData.moveSpeed)
        {
            float amount = Mathf.Min(Mathf.Abs(sm.rb.velocity.x), Mathf.Abs(sm.dData.frictionAmount));
            amount *= Mathf.Sign(sm.rb.velocity.x);
            sm.rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        // change to airborne (falling) in case of suddenly ungrounded, but has coyoteTime
        if (sm.pData.groundCoyoteTimeCounter < 0)
        {
            sm.states[(int)StateMachine.StateKey.Airborne].SwitchTo();
        }
    }

    protected override void OnExit()
    {

    } 
}
