using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BaseState
{
    private float rbGravityScale;
    private float timer = 0;
    protected override void OnEnter()
    {
        // if dash should go horizontally
        sm.rb.velocity = Vector2.zero;

        // no gravity while dashing
        rbGravityScale = sm.rb.gravityScale;
        sm.rb.gravityScale = 0;

        // dashingForce
        sm.rb.AddForce(Vector2.right * sm.horizontalMovement * sm.data.dashForce, ForceMode2D.Impulse);
        sm.data.isDashing = true;
        sm.tr.emitting = true;
    }

    protected override void OnUpdate()
    {
        timer = timer + Time.deltaTime;
        if (timer > sm.data.dashingTime)
        {
            sm.ChangeToLastState();
        }
    }

    protected override void OnExit()
    {
        sm.rb.gravityScale = rbGravityScale;
        timer = 0;
        sm.rb.velocity = Vector2.zero;
        sm.data.isDashing = false;
        sm.tr.emitting = false;
    }
}
