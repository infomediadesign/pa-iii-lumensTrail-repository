using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBaseState : BaseState
{
    public enum StateKey { Idle = 0, PickUp = 1, Carrying = 2, LightThrow = 3, LightWave = 4 };

    private static int allActionsUnlocked = 0;

    public ActionBaseState(StateMachine stateMachine) : base(stateMachine) 
    {
        ownState = StateKey.Idle; /*standartimplement to be sure*/
        stateType = BaseState.StateType.Action;
    }

    public override void SwitchTo()
    {
        base.SwitchTo();
    }
    public override void OnEnter()
    {
        if (!IsAllActionsUnlocked())
        {
            sm.ChangeState(StateKey.Idle);
            return;
        }
    }
    public static void LockAllActions()
    {
        allActionsUnlocked++;
        CursorManager.SetDisabledCursor();
    }
    public static void UnlockAllActions()
    {
        if (allActionsUnlocked > 0) allActionsUnlocked--;
        else throw new System.Exception("UnlockAllActions called without LockAllActions");
        CursorManager.SetNormalCursor();
    }
    public static bool IsAllActionsUnlocked()
    {
        return allActionsUnlocked == 0;
    }
}
