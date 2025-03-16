using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughPlatform : MonoBehaviour
{

    private Collider2D col;
    private PlatformEffector2D effector;
    private bool playerOnPlatform;
    [SerializeField] private float platformDeactiveTime = 0.5f;

    void Start()
    {
        col = GetComponent<Collider2D>();
        effector = GetComponent<PlatformEffector2D>();
    }
    void Update()
    {
        if (playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            //col.enabled = false;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), true);
            int layerToAdd = LayerMask.GetMask("Player");
            effector.colliderMask &= ~layerToAdd;
            StartCoroutine(EnablePlatformCollider());
        }
    }

    private IEnumerator EnablePlatformCollider()
    {
        yield return new WaitForSeconds(platformDeactiveTime);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), false);
        int layerToAdd = LayerMask.GetMask("Player");
        effector.colliderMask |= layerToAdd;
        //col.enabled = true;
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
