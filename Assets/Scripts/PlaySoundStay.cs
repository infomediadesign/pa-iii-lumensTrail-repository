using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundStay : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0, 1)] private float volume = 1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.SwitchSoundLoop(true);
        SoundManager.PlaySoundLoop(sound, volume);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.SwitchSoundLoop(false);
        SoundManager.SetSFXClipNull();
    }
}
