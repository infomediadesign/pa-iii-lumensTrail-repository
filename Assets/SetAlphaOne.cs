using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAlphaOne : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LumenThoughtBubble lumen = animator.gameObject.GetComponent<LumenThoughtBubble>();
        ThoughtBubble npc = animator.gameObject.GetComponentInParent<ThoughtBubble>();

        if (lumen != null) lumen.SetAlphaOne();
        else if (npc != null) npc.SetAlphaOne();
    }
}
