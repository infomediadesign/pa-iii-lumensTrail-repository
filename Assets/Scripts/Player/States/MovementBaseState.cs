using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBaseState : BaseState
{
    public enum StateKey { Still = 0, Moving = 1 };
    public static bool movementEnabled = true; //should it ever become a problem, that when multiple states disable it will be reset as soon
                                               // as the first state exits, then could make a Tuple or ValueTuple with an int counter (same for Gravity)
    public static float movementSpeedModifier = 1;
    protected Rigidbody2D rigidbody;

    public MovementBaseState(StateMachine stateMachine) : base(stateMachine) 
    { 
        ownState = StateKey.Still; /*standartimplement to be sure*/
        stateType = BaseState.StateType.Movement;
        rigidbody = stateMachine.rb;
    }


}
