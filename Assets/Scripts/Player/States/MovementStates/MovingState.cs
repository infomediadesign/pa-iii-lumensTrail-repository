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

    }

    public override void OnUpdate()
    {
        if (!movementEnabled) return;

        float targetSpeed = sm.horizontalMovement * sm.dData.moveSpeed * movementSpeedModifier;
        sm.rb.velocity = (Vector2.right * targetSpeed + Vector2.up * sm.rb.velocity);
    }

    public override void OnExit()
    {

    }
}
