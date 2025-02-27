using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasselbandeBridgeTrigger : MonoBehaviour
{
    public RasselbandeStartPathing pathing;

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        pathing.pathfinder.followEnabled = false;
        pathing.reachedBridge = true;
        
    }
}
