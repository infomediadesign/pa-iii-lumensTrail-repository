using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointTrigger : MonoBehaviour
{
    private VoidRespawn parent;

    void Start()
    {
        this.parent = GetComponentInParent<VoidRespawn>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            parent.SetRespawnPoint(this.transform);
        }
    }
}
