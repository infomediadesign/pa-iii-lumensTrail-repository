using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBaseState : BaseState
{
    public enum StateKey { Still = 0, Moving = 1 };
    private static int movementEnabled = 0; 
    public static float movementSpeedModifier = 1;
    protected Rigidbody2D rigidbody;

    public MovementBaseState(StateMachine stateMachine) : base(stateMachine) 
    { 
        ownState = StateKey.Still; /*standartimplement to be sure*/
        stateType = BaseState.StateType.Movement;
        rigidbody = stateMachine.rb;
    }

    public static void LockMovement()
    {
        movementEnabled++;
    }
    public static void UnlockMovement()
    {
        if (movementEnabled > 0) movementEnabled--;
    }

    public static bool IsMovementUnlocked()
    {
        return movementEnabled == 0;
    }


}
