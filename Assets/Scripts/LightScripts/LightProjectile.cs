using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightProjectile : MonoBehaviour
{
    private float projectileTravelTimeStart;
    public DesignerPlayerScriptableObject dData;
    [SerializeField] private ProgrammerPlayerScriptableObject pData;
    private Rigidbody2D rb;
    private Light2D pl;
    private SpriteRenderer sr;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pl = GetComponent<Light2D>();
        sr = GetComponent<SpriteRenderer>();
        // Get directional vector by subtracting current position from mouse pointer position
        Vector3 moveDirVec3 = pData.mousePositionOnLightThrow - transform.position;
        moveDirVec3.z = 0f;
        // moving projectile
        Vector2 moveDir = new Vector2(moveDirVec3.x, moveDirVec3.y).normalized;
        rb.velocity = moveDir * dData.lightThrowProjectileSpeed;
        
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 45f);  
    }

    private void OnBecameInvisible()
    {
        // Destroy projectile as soon as out of (camera) view
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;

        // code for activating InteractableObjects
        if (collision.CompareTag("LightThrowInteractable"))
        {
            BaseInteractableObject interactable = collision.GetComponent<BaseInteractableObject>();

            if (interactable != null)
            {
                interactable.Activate();
            }
        }
        // destory Projectile
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
