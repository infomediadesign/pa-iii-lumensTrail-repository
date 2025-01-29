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
