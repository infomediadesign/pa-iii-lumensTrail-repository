using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBaseState : BaseState
{
    public enum StateKey { Idle = 0, PickUp = 1, Carrying = 2, LightThrow = 3, LightWave = 4 };

    public ActionBaseState(StateMachine stateMachine) : base(stateMachine) 
    {
        ownState = StateKey.Idle; /*standartimplement to be sure*/
        stateType = BaseState.StateType.Action;
    }

    public override void SwitchTo()
    {
        if (!sm.pData.actionStateSwitchAllowed) return;
        base.SwitchTo();
    }
}
