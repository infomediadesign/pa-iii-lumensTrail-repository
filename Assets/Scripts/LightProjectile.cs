using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightProjectile : MonoBehaviour
{
    public DesignerPlayerScriptableObject dData;
    private Rigidbody2D rb;
    private float projectileTravelTimeStart;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 moveDirVec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 moveDirVec2 = new Vector2(moveDirVec3.x, moveDirVec3.y);
        rb.velocity = moveDirVec2.normalized * dData.lightThrowProjectileSpeed;
        projectileTravelTimeStart = Time.time;
    }

    private void Update()
    {
        if (Time.time > projectileTravelTimeStart + dData.lightThrowProjectileMaxTravelTime) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
