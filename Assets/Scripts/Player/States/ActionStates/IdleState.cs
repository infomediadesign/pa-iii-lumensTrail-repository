using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ActionBaseState
{
       public IdleState(StateMachine sm) : base(sm)
    {
        ownState = ActionBaseState.StateKey.Idle;
    }

    public override void SwitchTo()
    {
        base.SwitchTo();
    }

    public override void OnEnter()
    {
        
    }

    public override void OnUpdate()
    {
        
    }



}
