using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : BaseState
{
    private float rbGravityScale;
    private float fastFallingMultiplier;

    public AirborneState(StateMachine p_sm) : base(p_sm) 
    {
        stateKey = StateMachine.StateKey.Airborne;
    }

    public override void SwitchTo()
    {
        if (sm.currentState.stateKey == StateMachine.StateKey.Landing) return;
        base.SwitchTo();
    }

    protected override void OnEnter()
    {
        rbGravityScale = sm.rb.gravityScale;
        fastFallingMultiplier = sm.dData.fastFallingMultiplier;
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

        
        
        sm.rb.AddForce(Vector2.down * fastFallingMultiplier, ForceMode2D.Impulse);
        
        sm.pData.fallingVelocity = sm.rb.velocity.y;


        if (sm.pData.isTouchingWall)
        {
            sm.states[(int)StateMachine.StateKey.WallClinging].SwitchTo();
        }

        if (sm.pData.isGrounded)
        {
            sm.states[(int)StateMachine.StateKey.Grounded].SwitchTo();
        }
    }

    protected override void OnExit()
    {
        sm.rb.gravityScale = rbGravityScale;
    }
}
