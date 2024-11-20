using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BaseState
{
    private bool wallFlag=false;
    protected override void OnEnter()
    {
        sm.rb.AddForce(Vector2.up * sm.dData.jumpForce, ForceMode2D.Impulse);
        if(sm.pData.isTouchingWall) wallFlag=true;
    }

    protected override void OnUpdate()
    {
        // friction while in air
        if (!sm.pData.isGrounded && sm.rb.velocity.x < sm.dData.moveSpeed)
        {
            float amount = Mathf.Min(Mathf.Abs(sm.rb.velocity.x), Mathf.Abs(sm.dData.airFrictionAmount));
            amount *= Mathf.Sign(sm.rb.velocity.x);
            sm.rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        if (wallFlag && !sm.pData.isTouchingWall) wallFlag=false;

        if (sm.rb.velocity.y > 0 && !sm.pData.jumpButtonPressed)
        {
            sm.rb.AddForce(Vector2.down * sm.rb.velocity.y * (1 - sm.dData.jumpCutMultiplier), ForceMode2D.Impulse);
            sm.ChangeState(StateMachine.StateKey.Airborne);
        }

        if (sm.rb.velocity.y <= 0)
        {
            sm.ChangeState(StateMachine.StateKey.Airborne);
        }

        if (sm.pData.isTouchingWall && sm.lastState != sm.wallClState)
        {
            if(!wallFlag)sm.ChangeState(StateMachine.StateKey.WallClinging);
        }
    }

    protected override void OnExit()
    {

    }
}
