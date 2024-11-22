using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : BaseState
{
    
    public LandingState(StateMachine p_sm) : base(p_sm) 
    {
        stateKey = StateMachine.StateKey.Landing;
    }

    public override void SwitchTo()
    {
        if (sm.currentState.stateKey != StateMachine.StateKey.Airborne) return;
        base.SwitchTo();
    }

    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {

    }

    public override void OnMove()
    {

    }

    public override void OnExit()
    {

    }
}
