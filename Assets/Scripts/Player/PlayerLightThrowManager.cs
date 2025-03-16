using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightThrowManager : MonoBehaviour
{
    public Transform firePoint;
    public GameObject lightProjectilePrefab;
    public GameObject lightWavePrefab;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    public void LightThrow()
    {
        Instantiate(lightProjectilePrefab, firePoint.position, firePoint.rotation);
    }

    public void LightWave()
    {
        Instantiate(lightWavePrefab, firePoint.position, firePoint.rotation);
    }

    public void LightImpulse() 
    {
        LightImpuls lI = GetComponentInChildren<LightImpuls>();
        if (lI != null) lI.LightImpulse();
        else Debug.Log("LightImpulse Component is null");
        animator.SetBool("lightImpulse", false);
    }

}
