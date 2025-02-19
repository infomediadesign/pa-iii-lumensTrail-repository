using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkStump : MonoBehaviour
{
    private BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    public void DeactivateCollider()
    {
        this.col.enabled = false;
    }
}
