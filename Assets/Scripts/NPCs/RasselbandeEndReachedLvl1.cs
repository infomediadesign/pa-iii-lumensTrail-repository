using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasselbandeEndReachedLvl1 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
    }
}
