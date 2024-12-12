using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DashState : BaseState
{
    private float rbGravityScale;
    private float timer = 0;

    public DashState(StateMachine p_sm) : base(p_sm) 
    { 
        stateKey = StateMachine.StateKey.Dashing;
    }

    public override void OnEnter()
    {
        // if dash should go horizontally
        sm.rb.velocity = Vector2.zero;

        // no gravity while dashing
        rbGravityScale = sm.rb.gravityScale;
        sm.rb.gravityScale = 0;

        // dashingForce
        sm.rb.AddForce(Vector2.right * sm.horizontalMovement * sm.dData.dashForce, ForceMode2D.Impulse);
        sm.pData.isDashing = true;
        sm.tr.emitting = true;
    }

    public override void OnUpdate()
    {
        timer = timer + Time.deltaTime;
        if (timer > sm.dData.dashingTime)
        {
            sm.ChangeToLastState();
        }
    }

    public override void OnExit()
    {
        sm.rb.gravityScale = rbGravityScale;
        timer = 0;
        sm.rb.velocity = Vector2.zero;
        sm.pData.isDashing = false;
        sm.tr.emitting = false;
    }
}
