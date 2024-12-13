using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : PhysicsBaseState
{
    
    public LandingState(StateMachine p_sm) : base(p_sm) 
    {
        ownState = PhysicsBaseState.StateKey.Landing;
    }

    public override void SwitchTo()
    {
        if ((PhysicsBaseState.StateKey)sm.currentPhysicsState.ownState != PhysicsBaseState.StateKey.Airborne) return;
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
