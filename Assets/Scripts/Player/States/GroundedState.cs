using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GroundedState : BaseState
{
    protected override void OnEnter()
    {

    }

    protected override void OnUpdate()
    {
    }
    protected override void OnMove()
    {
        sm.characterController.Move(horizontalMovement);
    }
    protected override void OnExit()
    {
    } 
}
