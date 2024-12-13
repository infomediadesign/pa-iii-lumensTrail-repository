using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBaseState : BaseState
{
    public enum StateKey { Still = 0, Moving = 1 };
    public static bool movementEnabled = true;
    public static float movementSpeedModifier = 1;

    public MovementBaseState(StateMachine stateMachine) : base(stateMachine) 
    { 
        ownState = StateKey.Still; /*standartimplement to be sure*/
        stateType = BaseState.StateType.Movement;
    }


}
