using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightThrowManager : MonoBehaviour
{
    public Transform firePoint;
    public GameObject lightProjectilePrefab;
    public GameObject lightWavePrefab;

    public void LightThrow()
    {
        Instantiate(lightProjectilePrefab, firePoint.position, firePoint.rotation);
    }

    public void LightWave()
    {
        Instantiate(lightWavePrefab, firePoint.position, firePoint.rotation);
    }

}
