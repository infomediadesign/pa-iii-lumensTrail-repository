using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWaveState : BaseState
{
    public LightWaveState(StateMachine stateMachine) : base(stateMachine) 
    {
        stateKey = StateMachine.StateKey.LightWave;
    }

    public override void SwitchTo()
    {
        base.SwitchTo();
    }

    public override void OnEnter()
    {
        Debug.Log("LightWave");
    }

    public override void OnUpdate()
    {
        if (!sm.pData.lightThrowButtonPressed)
        {
            sm.states[(int)StateMachine.StateKey.Grounded].SwitchTo();
        }
    }

    public override void OnExit()
    {

    }

    public override void OnMove()
    {

    }
}
