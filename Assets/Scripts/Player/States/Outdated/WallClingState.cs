using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class WallClingState : BaseState
{
    private float rbGravityScale;
    private float timer = 1f;
    private float wallSlideGravityReduction = 2f;

    public WallClingState(StateMachine stateMachine) : base(stateMachine) 
    {
        stateKey = StateMachine.StateKey.WallClinging;

        timer = sm.dData.wallClingAirFreezeTime;
        wallSlideGravityReduction = sm.dData.wallClingSlideGravityReduction;
    }

    public override void SwitchTo()
    {
        if (!sm.hasLeftWallClState) return;
        base.SwitchTo();
    }

    public override void OnEnter()
    { 
        rbGravityScale = sm.rb.gravityScale;
        sm.rb.gravityScale = 0;
        sm.rb.velocity = Vector2.zero;
        sm.hasLeftWallClState = false;
    }

    public override void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (sm.pData.wallCoyoteTimeCounter > 0)
        {
            if (timer <= 0)
            {
                sm.rb.gravityScale = rbGravityScale / wallSlideGravityReduction;
            }
        }
        else
        {
            sm.states[(int)StateMachine.StateKey.Airborne].SwitchTo();
        }

        if (sm.pData.isGrounded)
        {
            sm.states[(int)StateMachine.StateKey.Grounded].SwitchTo();
        }
    }

    public override void OnExit()
    {
        sm.rb.gravityScale = rbGravityScale;
        timer = 0;
    }
}
*/