using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundStayNL : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField] private float volume = 1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.SwitchSoundLoopNL(true);
        SoundManager.PlaySoundLoopNL(sound, volume);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.SwitchSoundLoopNL(false);
        SoundManager.SetSFXNLClipNull();
    }
}
