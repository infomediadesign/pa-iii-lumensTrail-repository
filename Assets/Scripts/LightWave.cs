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
    private Vector3 desiredLocalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pl = GetComponent<Light2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
        // Get directional vector by subtracting current position from mouse pointer position
        Vector3 moveDirVec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // moving projectile
        rb.velocity = new Vector2(moveDirVec3.x, moveDirVec3.y).normalized * dData.lightWaveSpeed;
        // Reduced size at start
        startScale = dData.lightWaveStartingSizeMultiplier;
        desiredLocalScale = transform.localScale;
        transform.localScale = desiredLocalScale * startScale;
        startPos = transform.position;

        // rotate wave to destination
        float angle = Mathf.Atan2(moveDirVec3.y, moveDirVec3.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // need to check if Start() has already been called or nah, cause sometimes
        // this mf thinks it has to collide before even being instanciated completly
        if (rb == null) return;
        
        if (collision.tag == "Player") return;
        /*
         * Code for projectile interacting with Objects here 
         */
        StartCoroutine(DestroyLightWave());
    }

    IEnumerator DestroyLightWave()
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
