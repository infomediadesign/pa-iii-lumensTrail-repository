using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightProjectile : MonoBehaviour
{
    private float projectileTravelTimeStart;
    public DesignerPlayerScriptableObject dData;
    private Rigidbody2D rb;
    private Light2D pl;
    private SpriteRenderer sr;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pl = GetComponent<Light2D>();
        sr = GetComponent<SpriteRenderer>();
        // Get directional vector by subtracting current position from mouse pointer position
        Vector3 moveDirVec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // moving projectile
        rb.velocity = new Vector2(moveDirVec3.x, moveDirVec3.y).normalized * dData.lightThrowProjectileSpeed;
        projectileTravelTimeStart = Time.time;
    }

    private void Update()
    {
        // Destory projectile after a certain amount of time, in case it never hits anything
        if (Time.time > projectileTravelTimeStart + dData.lightThrowProjectileMaxTravelTime) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;
        /*
         * Code for projectile interacting with Objects here 
         */
        StartCoroutine(DestroyProjectile());
    }

    IEnumerator DestroyProjectile()
    {
        // possible behavior of projectile when getting destroyed
        sr.enabled = false;
        pl.intensity = pl.intensity * 3f;
        pl.pointLightOuterRadius = pl.pointLightOuterRadius * 2f;
        pl.pointLightInnerRadius = pl.pointLightInnerRadius * 2f;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}
