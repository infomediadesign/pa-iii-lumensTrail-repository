using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightThrowState : BaseState
{
    float lightThrowButtonHoldTimer;
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
        lightThrowButtonHoldTimer = Time.time;
    }

    public override void OnUpdate()
    {
        if (Time.time < lightThrowButtonHoldTimer + sm.dData.startChargingDelay)
        {
            if (!sm.pData.lightThrowButtonPressed)
            {
                sm.ltm.LightThrow();
                sm.states[(int)StateMachine.StateKey.Grounded].SwitchTo();
            }
        }
        else
        {
            sm.states[(int)StateMachine.StateKey.LightWave].SwitchTo();
        }
    }

    public override void OnExit()
    {

    }

    public override void OnMove()
    {
        if (Time.time < lightThrowButtonHoldTimer + sm.dData.startChargingDelay) base.OnMove();
    }
}
