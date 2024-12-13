using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBaseState : BaseState
{
    public enum StateKey { Idle, PickUp, Carrying, LightThrow, LightWave };

    public ActionBaseState(StateMachine stateMachine) : base(stateMachine) { ownState = StateKey.Idle; /*standartimplement to be sure*/ }


}
