using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterBarrier : MonoBehaviour
{
    private CameraFollow cam;
    [SerializeField] private bool leftBarrier = true;
 
    private void Start()
    {
        cam = FindObjectOfType<CameraFollow>();
    }

    private void OnBecameVisible()
    {
        if (leftBarrier) cam.SetStopFollowingLeft(true);
        else cam.SetStopFollowingRight(true);
    }
}
