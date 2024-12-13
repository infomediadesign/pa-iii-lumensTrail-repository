using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBaseState : BaseState
{
    
    public enum StateKey { Grounded, Jumping, Airborne, Landing };

    public static float gravityModifier = 1;
    
    public PhysicsBaseState(StateMachine stateMachine) : base(stateMachine) { ownState = StateKey.Grounded; /*standartimplement to be sure*/ }




}
