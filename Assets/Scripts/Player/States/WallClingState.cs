using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClingState : BaseState
{
    private float rbGravityScale;
    private float timer = 0;
    protected override void OnEnter()
    {
       rbGravityScale = sm.rb.gravityScale;
        sm.rb.gravityScale = 0;
        sm.rb.velocity = Vector2.zero;
    }

    protected override void OnUpdate()
    {
        timer = timer + Time.deltaTime;
        if (sm.data.isTouchingWall)
        {
            if (timer >= 1)
            {
                sm.rb.gravityScale = rbGravityScale / 2;
            }
        }
        else
        {
            sm.ChangeState(StateMachine.StateKey.Airborne);
        }

        if (sm.data.isGrounded)
        {
            sm.ChangeState(StateMachine.StateKey.Grounded);
        }
    }

    protected override void OnExit()
    {
        sm.rb.gravityScale = rbGravityScale;
        timer = 0;
        Debug.Log("Leaving WallCling State");
    }
}
