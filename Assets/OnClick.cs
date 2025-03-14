using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    public void OnClickMouse()
    {
        SoundManager.PlaySoundNL(SoundType.UICLICK, 1);
    }
}
