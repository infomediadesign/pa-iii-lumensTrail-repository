using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightThrowState : BaseState
{
    float lightThrowButtonHoldTimer;
    private float rbGravityScale;
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
        rbGravityScale = sm.rb.gravityScale;
        sm.rb.gravityScale = sm.dData.lightThrowGravityMultiplier;
    }

    public override void OnUpdate()
    {
        // Checking if enough time has passed to switch to light wave mechanic
        if (Time.time < lightThrowButtonHoldTimer + sm.dData.switchToLightWaveTime)
        {
            if (!sm.pData.lightThrowButtonPressed)
            {
                // Calling the LightThrowManager to create an instance of the projectile
                sm.ltm.LightThrow();
                sm.states[(int)StateMachine.StateKey.Grounded].SwitchTo();
            }
        }
        else
        {
            if (sm.pData.isGrounded)
            {
                // after the time passed, automatically switch to light wave state
                sm.states[(int)StateMachine.StateKey.LightWave].SwitchTo();
            }
            else
            {
                sm.states[(int)StateMachine.StateKey.Grounded].SwitchTo();
            }
        }
    }

    public override void OnExit()
    {
        sm.rb.gravityScale = rbGravityScale;
    }

    public override void OnMove()
    {
        // let the player move until charging is starting
        if (Time.time > lightThrowButtonHoldTimer + sm.dData.startChargingDelay && sm.pData.isGrounded) 
        {
            sm.rb.velocity = Vector2.zero;
            return;
        }
        base.OnMove();
    }
}
