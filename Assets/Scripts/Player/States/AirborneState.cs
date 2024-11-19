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
        // friction while in air
        if (!sm.pData.isGrounded && sm.rb.velocity.x < sm.dData.moveSpeed)
        {
            float amount = Mathf.Min(Mathf.Abs(sm.rb.velocity.x), Mathf.Abs(sm.dData.airFrictionAmount));
            amount *= Mathf.Sign(sm.rb.velocity.x);
            sm.rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        if (sm.rb.velocity.y <= 0)
        {
            sm.rb.gravityScale = rbGravityScale * sm.dData.fallGravityMultiplier;
        }

        if (sm.pData.isTouchingWall)
        {
            sm.ChangeState(StateMachine.StateKey.WallClinging);
        }

        if (sm.pData.isGrounded)
        {
            sm.ChangeState(StateMachine.StateKey.Grounded);
        }
    }

    protected override void OnExit()
    {
        sm.rb.gravityScale = rbGravityScale;
    }
}
