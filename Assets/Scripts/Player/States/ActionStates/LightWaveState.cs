using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightWaveState : ActionBaseState
{
    private Light2D light;
    private Color lightOriginalColor;
    private bool lightWaveFired;
    private float timer;
    public LightWaveState(StateMachine stateMachine) : base(stateMachine) 
    {
        ownState = ActionBaseState.StateKey.LightWave;
    }

    public override void SwitchTo()
    {
        if ((ActionBaseState.StateKey)sm.currentActionState.ownState != ActionBaseState.StateKey.Idle) return;
        base.SwitchTo();
    }

    public override void OnEnter()
    {
        /*
        light = sm.GetComponent<Light2D>();
        if (light == null) Debug.Log("Light is null");
        lightOriginalColor = light.color;
        light.color = Color.blue;
        */
        base.OnEnter();
        sm.rb.velocity = Vector2.zero;
        this.timer = 0;
        lightWaveFired = false;
        sm.animator.SetBool("lightWave", true);
        MovementBaseState.LockMovement();
    }

    public override void OnUpdate()
    {
        if (!lightWaveFired)
        {
            if (!sm.pData.lightThrowButtonPressed)
            {
                sm.animator.SetBool("lightWave", false);
                timer = Time.time;
                this.lightWaveFired = true;
                // so light instantly returns to original color, instead of when player leaves the state
                //this.light.color = this.lightOriginalColor;
            }
        }
        else
        {
            if (Time.time > timer + sm.dData.lightWaveMovementDisabledTime) sm.SwitchToState(ActionBaseState.StateKey.Idle);
        }
    }

    public override void OnExit()
    {
        // change color as soon as leaving state
        //light.color = lightOriginalColor;
        lightWaveFired = false;
        MovementBaseState.UnlockMovement();
    }  
}
