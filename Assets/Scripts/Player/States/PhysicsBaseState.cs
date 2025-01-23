using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBaseState : BaseState
{
    
    public enum StateKey { Grounded = 0, Jumping = 1, Airborne = 2, Landing = 3 };

    public static float gravityModifier = 1;
    public static bool enableGravity = true;
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
        rigidbody.gravityScale = !enableGravity ? 0 : standartGravity * gravityModifier;
    }


}
