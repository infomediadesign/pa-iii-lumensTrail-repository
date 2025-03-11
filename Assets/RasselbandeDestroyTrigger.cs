using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasselbandeDestroyTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<RasselbandeStartWalking>() != null) Destroy(collision.gameObject);
    }
}
