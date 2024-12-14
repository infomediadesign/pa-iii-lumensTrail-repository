using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightWaveState : ActionBaseState
{
    private Light2D light;
    private Color lightOriginalColor;
    public LightWaveState(StateMachine stateMachine) : base(stateMachine) 
    {
        ownState = ActionBaseState.StateKey.LightWave;
    }

    public override void SwitchTo()
    {
        base.SwitchTo();
    }

    public override void OnEnter()
    {
        light = sm.GetComponent<Light2D>();
        if (light == null) Debug.Log("Light is null");
        lightOriginalColor = light.color;
        light.color = Color.blue;
        MovementBaseState.movementEnabled = false;
    }

    public override void OnUpdate()
    {
        if (!sm.pData.lightThrowButtonPressed)
        {
            sm.ltm.LightWave();
            sm.SwitchToState(ActionBaseState.StateKey.Idle);
        }
    }

    public override void OnExit()
    {
        light.color = lightOriginalColor;
        MovementBaseState.movementEnabled = true;
    }

    
}
