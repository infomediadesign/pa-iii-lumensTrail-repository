using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightTrigger : MonoBehaviour
{
    [SerializeField] private float maxLightIntensity;
    private float baseLightIntensity;
    [SerializeField] Light2D globalLight;

    private float minY;
    private float maxY;
    public float lerpIntensity = 5f;

    void Start()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        minY = box.bounds.min.y;
        maxY = box.bounds.max.y;
        baseLightIntensity = globalLight.intensity;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            float playerY = collision.transform.position.y;
            float t = Mathf.InverseLerp(minY, maxY, playerY);
            //globalLight.intensity = Mathf.Lerp(baseLightIntensity, maxLightIntensity, t);
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, Mathf.Lerp(baseLightIntensity, maxLightIntensity, t), Time.deltaTime * lerpIntensity);
        }
    }
}
