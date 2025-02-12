using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterBarrier : MonoBehaviour
{
    private CameraFollow cam;
    private bool stopFollow = false;

    private void Start()
    {
        cam = FindObjectOfType<CameraFollow>();
    }

    private void Update()
    {
        
    }

    private void OnBecameVisible()
    {
        cam.SetStopFollowing(true);
    }
}
