using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAlphaZero : StateMachineBehaviour
{
   public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LumenThoughtBubble lumen = animator.gameObject.GetComponent<LumenThoughtBubble>();
        ThoughtBubble npc = animator.gameObject.GetComponentInParent<ThoughtBubble>();

        if (lumen != null) lumen.SetAlphaZero();
        else if (npc != null) npc.SetAlphaZero();
    }
}
