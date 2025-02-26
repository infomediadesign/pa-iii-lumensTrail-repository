using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAlphaZero : StateMachineBehaviour
{
   public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<LumenThoughtBubble>().SetAlphaZero();
    }
}
