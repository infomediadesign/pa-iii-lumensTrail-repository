using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseRangeCheck : MonoBehaviour
{
    public bool inRange {private set;get;}= false;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        inRange = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        inRange = false;
    }
}
