using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidRespawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPos;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            collision.transform.position = respawnPos.position;
        }
    }
}
