using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : MovementBaseState
{
    public MovingState(StateMachine p_sm) : base(p_sm)
    {
        ownState = MovementBaseState.StateKey.Moving;
    }

    public override void SwitchTo()
    {
        base.SwitchTo();
    }


    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        if (!IsMovementUnlocked()) return;

        float targetSpeed = sm.horizontalMovement * sm.dData.moveSpeed * movementSpeedModifier;
        rigidbody.velocity = (Vector2.right * targetSpeed + Vector2.up * rigidbody.velocity);
        sm.animator.SetFloat("horizontalSpeed", (float)Math.Sqrt(targetSpeed * targetSpeed));
    }

    public override void OnExit()
    {
        rigidbody.velocity = Vector2.up * rigidbody.velocity;
        sm.animator.SetFloat("horizontalSpeed", 0);
    }
}
