using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : BaseState
{
    public AttackingState(StateMachine p_sm) : base(p_sm)
    {
        stateKey = StateMachine.StateKey.Attacking;
    }

    public override void SwitchTo()
    {
        base.SwitchTo();
    }

    protected override void OnEnter()
    {

    }

    protected override void OnUpdate()
    {

    }

    protected override void OnMove()
    {

    }

    protected override void OnExit()
    {

    }
}
