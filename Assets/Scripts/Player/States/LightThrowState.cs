using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightThrowState : BaseState
{
    float timer = 0;
    public LightThrowState(StateMachine stateMachine) : base(stateMachine) 
    {
        stateKey = StateMachine.StateKey.LightThrow;
    }

    public override void SwitchTo()
    {
        base.SwitchTo();
    }

    public override void OnEnter()
    {
        sm.ltm.LightThrow();
    }

    public override void OnUpdate()
    {
        timer++;
        if (timer > 120)
        {
            sm.states[(int)StateMachine.StateKey.Grounded].SwitchTo();
        }
    }

    public override void OnExit()
    {

    }
}
