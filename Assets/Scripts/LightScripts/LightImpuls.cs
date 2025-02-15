using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightImpuls : MonoBehaviour
{
    public DesignerPlayerScriptableObject dData;
    private bool isActive;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private float currentLerpTime;
    private Light2D lt;
    private float ltOuterRadiusOriginal;
    private float ltOuterRadiusMax;
    private bool reduceLightRadius = false;
    private Animator animator;

    void Start()
    {
        initialScale = Vector3.zero;
        transform.localScale = initialScale;
        targetScale = new Vector3(dData.maxImpulseRadius, dData.maxImpulseRadius, 0);
        isActive = false;
        currentLerpTime = 0;
        lt = GetComponentInParent<Light2D>();
        ltOuterRadiusOriginal = lt.pointLightOuterRadius;
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (isActive)
        {
            // Erhalte die relative Skalierung des Kreises basierend auf der lossyScale des Spielers
            Vector3 playerScale = transform.parent.lossyScale; // Spieler ist das Elternobjekt des Kreises
            float scaleFactorX = 1f / playerScale.x;
            float scaleFactorY = 1f / playerScale.y;

            if (transform.localScale.x < dData.maxImpulseRadius)
            {
                currentLerpTime += Time.deltaTime * dData.impulseSpeed;
                float lerpValue = Mathf.Lerp(0, dData.maxImpulseRadius, currentLerpTime);

                // Setze die relative Skalierung, um die Verzerrung auszugleichen
                transform.localScale = new Vector3(lerpValue * scaleFactorX, lerpValue * scaleFactorY, transform.localScale.z);
                if (lerpValue > lt.pointLightOuterRadius) lt.pointLightOuterRadius = lerpValue;
            }
            else
            {
                transform.localScale = initialScale;
                currentLerpTime = 0;
                isActive = false;
                animator.SetBool("lightImpulse", false);
                StartCoroutine(ReduceLightEmittingRadius());
            }
        }

        if (reduceLightRadius)
        {
            if (lt.pointLightOuterRadius > ltOuterRadiusOriginal)
            {
                currentLerpTime += Time.deltaTime * dData.impulseSpeed;
                float lerpValue = Mathf.Lerp(ltOuterRadiusMax, ltOuterRadiusOriginal, currentLerpTime);
                lt.pointLightOuterRadius = lerpValue;
            }
            else
            {
                lt.pointLightOuterRadius = ltOuterRadiusOriginal;
                currentLerpTime = 0;
                reduceLightRadius = false;
            }
        }
    }

    private IEnumerator ReduceLightEmittingRadius()
    {
        yield return new WaitForSeconds(dData.increasedLightRadiusTime);
        ltOuterRadiusMax = lt.pointLightOuterRadius;
        reduceLightRadius = true;
    }

    public void LightImpulse()
    {
        isActive = true;
        animator.SetBool("lightImpulse", true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;
        BaseInteractableObject interactable = collision.GetComponent<BaseInteractableObject>();
        if (interactable != null) interactable.GlowOn();
        else Debug.Log("Interactable is null");
    }
}
