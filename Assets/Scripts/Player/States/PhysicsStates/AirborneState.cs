using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PhysicsBaseState
{
    private float rbGravityScale;
    private float fastFallingMultiplier;

    public AirborneState(StateMachine p_sm) : base(p_sm) 
    {
        ownState = PhysicsBaseState.StateKey.Airborne;
        //fastFallingMultiplier = sm.dData.fastFallingMultiplier;

    }

    public override void SwitchTo()
    {
        if ((PhysicsBaseState.StateKey)sm.currentPhysicsState.ownState == PhysicsBaseState.StateKey.Landing) return;
        base.SwitchTo();
    }

    public override void OnEnter()
    {
       /* rbGravityScale = sm.rb.gravityScale;
        sm.rb.gravityScale = rbGravityScale * sm.dData.fallGravityMultiplier;*/

        gravityModifier = gravityModifier * sm.dData.fallGravityMultiplier;
        MovementBaseState.movementSpeedModifier *= sm.dData.airFrictionAmount;
        sm.rb.sharedMaterial = sm.slip;
        sm.animator.SetBool("airborne", true);
        base.OnEnter();
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

        
        //sm.rb.AddForce(Vector2.down * fastFallingMultiplier, ForceMode2D.Impulse);
        
        sm.pData.fallingVelocity = sm.rb.velocity.y;
        sm.animator.SetFloat("verticalSpeed", sm.rb.velocity.y);

        /*
         * @outdated and to be removed
         **/

        /*if (sm.pData.isTouchingWall)
        {
            sm.states[(int)StateMachine.StateKey.WallClinging].SwitchTo();
        }*/

        if (sm.pData.isGrounded)
        {
            sm.SwitchToState(PhysicsBaseState.StateKey.Grounded);
        }
    }

    public override void OnExit()
    {
        //sm.rb.gravityScale = rbGravityScale;
        gravityModifier = gravityModifier / sm.dData.fallGravityMultiplier;
        MovementBaseState.movementSpeedModifier /= sm.dData.airFrictionAmount;
        sm.rb.sharedMaterial = sm.normal;
        sm.animator.SetBool("airborne", false);
        sm.animator.SetFloat("verticalSpeed", 0);
    }
}
