using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightWave : MonoBehaviour
{
    public DesignerPlayerScriptableObject dData;
    private Rigidbody2D rb;
    private Light2D pl;
    private SpriteRenderer sr;
    private CapsuleCollider2D col;
    private Vector3 startPos;
    private float startScale;
    private float maxScale = 1f;
    public Vector3 desiredLocalScale { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pl = GetComponent<Light2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
        
        bool isFacingRight = FindObjectOfType<PlayerController>().GetIsFacingRight();
        float directionalFloat;
        if (isFacingRight) directionalFloat = 1;
        else directionalFloat = -1;
        
        // moving light wave
        rb.velocity = Vector2.right * directionalFloat * dData.lightWaveSpeed;
        // Reduced size at start
        startScale = dData.lightWaveStartingSizeMultiplier;
        desiredLocalScale = transform.localScale;
        transform.localScale = desiredLocalScale * startScale;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // current traveled distance
        float distance = Vector3.Distance(startPos, transform.position);
        if (transform.localScale != desiredLocalScale)
        {
            // calculating growth
            float t = Mathf.Clamp01(distance / dData.lightWaveMaxSizeAtDistance);
            float currentScale = Mathf.Lerp(startScale, maxScale, t);

            transform.localScale = desiredLocalScale * currentScale;
        }
        // destroy light wave at max traveled distance
        if (distance > dData.lightWaveMaxTravelDistance) StartCoroutine(DestroyLightWave());
    }

    public IEnumerator DestroyLightWave()
    {
        // possible behavior of projectile when getting destroyed
        //sr.enabled = false;
        //pl.intensity = pl.intensity * 3f;
        //pl.pointLightOuterRadius = pl.pointLightOuterRadius * 2f;
        //pl.pointLightInnerRadius = pl.pointLightInnerRadius * 2f;
        //rb.velocity = Vector2.zero;
        yield return null;
        Destroy(gameObject);
    }

}
