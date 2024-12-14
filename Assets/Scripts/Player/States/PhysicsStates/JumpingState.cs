using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PhysicsBaseState
{
    private bool wallFlag=false;


    public JumpingState(StateMachine sm) : base(sm) 
    {
        ownState = PhysicsBaseState.StateKey.Jumping;
    }

    public override void SwitchTo()
    {
        if ((PhysicsBaseState.StateKey)sm.currentPhysicsState.ownState == PhysicsBaseState.StateKey.Airborne) return;
        if ((ActionBaseState.StateKey)sm.currentActionState.ownState == ActionBaseState.StateKey.LightWave) return; //should disable jumping during lightwave charge
        base.SwitchTo();
    }

    public override void OnEnter()
    {
        sm.rb.AddForce(Vector2.up * sm.dData.jumpForce, ForceMode2D.Impulse);
        if(sm.pData.isTouchingWall) wallFlag=true; //outdated, to be removed

        MovementBaseState.movementSpeedModifier *= sm.dData.airFrictionAmount;
    }

    public override void OnUpdate()
    {
        /*
         * @outdated: instead uses MovementSpeedModifier now
         */

        /*// friction while in air
        if (!sm.pData.isGrounded && sm.rb.velocity.x < sm.dData.moveSpeed)
        {
            float amount = Mathf.Min(Mathf.Abs(sm.rb.velocity.x), Mathf.Abs(sm.dData.airFrictionAmount));
            amount *= Mathf.Sign(sm.rb.velocity.x);
            sm.rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }*/




        if (wallFlag && !sm.pData.isTouchingWall) wallFlag=false; //outdated and to be removed

        if (sm.rb.velocity.y > 0 && !sm.pData.jumpButtonPressed)
        {
            sm.rb.AddForce(Vector2.down * sm.rb.velocity.y * (1 - sm.dData.jumpCutMultiplier), ForceMode2D.Impulse);
            sm.SwitchToState(PhysicsBaseState.StateKey.Airborne);
        }

        if (sm.rb.velocity.y <= 0)
        {
            sm.SwitchToState(PhysicsBaseState.StateKey.Airborne);
        }

        /**
         * @outdated and to be removed
         **/

        /*if (sm.pData.isTouchingWall && sm.lastState != sm.states[(int)StateMachine.StateKey.WallClinging])
        {
            if(!wallFlag)sm.states[(int)StateMachine.StateKey.WallClinging].SwitchTo();
        }*/
    }

    public override void OnExit()
    {
        MovementBaseState.movementSpeedModifier /= sm.dData.airFrictionAmount;
    }
}
