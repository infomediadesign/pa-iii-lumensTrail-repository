using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughPlatform : MonoBehaviour
{

    private Collider2D collider;
    private bool playerOnPlatform;
    [SerializeField] private float platformDeactiveTime;

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }
    void Update()
    {
        if (playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            collider.enabled = false;
            StartCoroutine(EnablePlatformCollider());
        }
    }

    private IEnumerator EnablePlatformCollider()
    {
        yield return new WaitForSeconds(platformDeactiveTime);
        collider.enabled = true;
    }

    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        var player = other.gameObject.GetComponent<PlayerController>();

        if (player != null) 
        { 
            playerOnPlatform = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision, false);
    }
}
