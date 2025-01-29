using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StillState : MovementBaseState
{
    public StillState(StateMachine p_sm) : base(p_sm)
    {
        ownState = MovementBaseState.StateKey.Still;
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

    public override void OnExit()
    {

    }
}
