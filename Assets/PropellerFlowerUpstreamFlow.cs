using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerFlowerUpstreamFlow : MonoBehaviour
{
    private PropellerFlower parent;

    private void Start()
    {
        parent = GetComponentInParent<PropellerFlower>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (parent != null)
        {
            parent.OnPlayerStayWindTrigger(collision);
        }
    }
}
