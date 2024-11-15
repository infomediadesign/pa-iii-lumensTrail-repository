using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : BaseState
{
    private float rbGravityScale;
    protected override void OnEnter()
    {
        rbGravityScale = sm.rb.gravityScale;
    }

    protected override void OnUpdate()
    {
        if (sm.rb.velocity.y <= 0)
        {
            sm.rb.gravityScale = rbGravityScale * sm.data.fallGravityMultiplier;
        }

        if (sm.data.isGrounded)
        {
            sm.ChangeState(StateMachine.StateKey.Grounded); 
        }
    }

    protected override void OnExit()
    {
        sm.rb.gravityScale = rbGravityScale;
        Debug.Log("Change state to grounded");
    }
}
