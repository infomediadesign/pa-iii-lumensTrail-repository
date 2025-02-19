using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightThrowState : ActionBaseState
{
    float lightThrowButtonHoldTimer;
    private float rbGravityScale;
    public LightThrowState(StateMachine stateMachine) : base(stateMachine) 
    {
        ownState = ActionBaseState.StateKey.LightThrow;
    }

    public override void SwitchTo()
    {
        if ((ActionBaseState.StateKey)sm.currentActionState.ownState != ActionBaseState.StateKey.Idle) return;
        if((ActionBaseState.StateKey)sm.currentActionState.ownState != ActionBaseState.StateKey.Idle) return;
        base.SwitchTo();
    }

    public override void OnEnter()
    {
        lightThrowButtonHoldTimer = Time.time;
        PhysicsBaseState.gravityModifier *= sm.dData.lightThrowGravityMultiplier;
        sm.animator.SetBool("lightThrow", true);
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
                sm.animator.SetBool("lightThrow", false);
                sm.SwitchToState(ActionBaseState.StateKey.Idle);
            }
        }
        else
        {
            if (sm.pData.isGrounded)
            {
                // after the time passed, automatically switch to light wave state
                sm.SwitchToState(ActionBaseState.StateKey.LightWave);
            }
            else
            {
                sm.animator.SetBool("lightThrow", false);
                sm.SwitchToState(ActionBaseState.StateKey.Idle);
            }
        }

        /***
         * @attention: should probably be looked into, the charge is not really implemented, it just disables movement, as far as i can tell 
         ***/
        if (Time.time > lightThrowButtonHoldTimer + sm.dData.startChargingDelay && (PhysicsBaseState.StateKey)sm.currentPhysicsState.ownState == PhysicsBaseState.StateKey.Grounded)
        {
            MovementBaseState.movementEnabled = false;
        }

    }

    public override void OnExit()
    {
        PhysicsBaseState.gravityModifier /= sm.dData.lightThrowGravityMultiplier;
        MovementBaseState.movementEnabled = true;
        sm.animator.SetBool("lightThrow", false);
    }

    /***
     * @outdated: now in OnUpdate() 
     ***/
    
    /*public override void OnMove()
    {
        // let the player move until charging is starting
        if (Time.time > lightThrowButtonHoldTimer + sm.dData.startChargingDelay && sm.pData.isGrounded) 
        {
            sm.rb.velocity = Vector2.zero;
            return;
        }
        base.OnMove();
    }*/
}
