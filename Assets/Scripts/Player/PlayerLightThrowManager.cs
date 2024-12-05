using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightThrowManager : MonoBehaviour
{
    public Transform firePoint;
    public GameObject lightProjectilePrefab;

    public void LightThrow()
    {
        Instantiate(lightProjectilePrefab, firePoint.position, firePoint.rotation);
    }

}
