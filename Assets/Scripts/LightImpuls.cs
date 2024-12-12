using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightImpuls : MonoBehaviour
{
    public DesignerPlayerScriptableObject dData;
    private bool isActive;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private float currentLerpTime;

    void Start()
    {
        initialScale = Vector3.zero;
        transform.localScale = initialScale;
        targetScale = new Vector3(dData.maxImpulseRadius, dData.maxImpulseRadius, 0);
        isActive = false;
        currentLerpTime = 0;
    }

    void Update()
    {
        if (isActive)
        {
            if (transform.localScale.x < dData.maxImpulseRadius)
            {
                currentLerpTime += Time.deltaTime * dData.impulseSpeed;
                transform.localScale = Vector3.Lerp(initialScale, targetScale, currentLerpTime);
            }
            else
            {
                transform.localScale = initialScale;
                currentLerpTime = 0;
                isActive = false;
            }
        }
    }

    public void LightImpulse()
    {
        isActive = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        BaseInteractableObject interactable = collision.GetComponent<BaseInteractableObject>();
        if (interactable != null) interactable.GlowOn();
        else Debug.Log("Interactable is null");
    }
}
