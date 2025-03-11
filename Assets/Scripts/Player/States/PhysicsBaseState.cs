using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBaseState : BaseState
{
    
    public enum StateKey { Grounded = 0, Jumping = 1, Airborne = 2, Landing = 3 };

    public static float gravityModifier = 1;
    private static int enableGravity = 0;
    protected readonly float standartGravity;
    protected Rigidbody2D rigidbody;
    
    public PhysicsBaseState(StateMachine stateMachine) : base(stateMachine)
    { 
        ownState = StateKey.Grounded; /*standartimplement to be sure*/
        standartGravity = sm.dData.generalGravityMultiplier;
        stateType = BaseState.StateType.Physics;
    }

    public override void OnEnter()
    {
        rigidbody = sm.rb;
    }
    public override void OnUpdate()
    {
        rigidbody.gravityScale = !IsGravityUnlocked() ? 0 : standartGravity * gravityModifier;
    }

    public static void LockGravity()
    {
        enableGravity++;
    }
    public static void UnlockGravity()
    {
        if (enableGravity > 0) enableGravity--;
        else throw new System.Exception("Movement was already unlocked");
    }

    public static bool IsGravityUnlocked()
    {
        return gravityModifier == 0;
    }



}
