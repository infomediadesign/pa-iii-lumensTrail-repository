using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class KekeMaxJumpTrigger : MonoBehaviour
{
    [SerializeField] private KekeAIAdvanced kekeAI;
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("KekeJumpTarget"))
        {
            GameObject parent = collision.transform.parent.transform.parent.gameObject;
            if (parent == null) parent = collision.transform.parent.gameObject;
            if (parent == null || (parent.layer != LayerMask.NameToLayer("Ground") && parent.layer != LayerMask.NameToLayer("Platform"))) throw new Exception("No correct Parent");

            Tuple<Collider2D, GameObject> jumpPoint = new Tuple<Collider2D, GameObject>(collision, parent);
            kekeAI.possibleTargetsWithPlatforms.Add(jumpPoint);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("KekeJumpTarget"))return;
        for (int i = kekeAI.possibleTargetsWithPlatforms.Count - 1; i >= 0; i--)
        {
            if (kekeAI.possibleTargetsWithPlatforms[i].Item1 == collision)
            {
                kekeAI.possibleTargetsWithPlatforms.RemoveAt(i);
            }
        }
    }
}
