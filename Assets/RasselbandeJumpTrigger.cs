using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasselbandeJumpTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Erkannt");
        RasselbandeStartWalking bande = collision.GetComponent<RasselbandeStartWalking>();
        if (bande != null)
        {
            bande.OnJump();
            Debug.Log("Jump");
        }
    }
}
