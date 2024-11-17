using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BaseState
{
    protected override void OnEnter()
    {
        sm.rb.AddForce(Vector2.up * sm.data.jumpForce, ForceMode2D.Impulse);
    }

    protected override void OnUpdate()
    {
        if (sm.rb.velocity.y > 0 && !sm.data.jumpButtonPressed)
        {
            sm.rb.AddForce(Vector2.down * sm.rb.velocity.y * (1 - sm.data.jumpCutMultiplier), ForceMode2D.Impulse);
            sm.ChangeState(StateMachine.StateKey.Airborne);
        }

        if (sm.rb.velocity.y <= 0)
        {
            sm.ChangeState(StateMachine.StateKey.Airborne);
        }

        if (sm.data.isTouchingWall && sm.lastState != sm.wallClState)
        {
            sm.ChangeState(StateMachine.StateKey.WallClinging);
        }
    }

    protected override void OnExit()
    {

    }
}
