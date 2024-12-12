using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWaveCollider : MonoBehaviour
{
    private LightWave parent;
    private CapsuleCollider2D col;
    private Vector2 parentOriginalScale;
    void Start()
    {
        parent = GetComponentInParent<LightWave>();
        col = GetComponent<CapsuleCollider2D>();
        col.size = parent.dData.lightWaveColliderSize;
        parentOriginalScale = parent.desiredLocalScale;
    }

    void Update()
    {
        if (parent == null) return;
        // get current scale of parent transform
        Vector2 parentScale = parent.transform.localScale;
        // calculate the adjusted size based on current scale of parent, so the child scale stays the same scale 
        Vector2 adjustedScale = new Vector2(parentOriginalScale.x / parentScale.x, parentOriginalScale.y / parentScale.y);
        transform.localScale = adjustedScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // need to check if Start() has already been called or nah, cause sometimes
        // this mf thinks it has to collide before even being instanciated completly
        if (parent == null) return;

        //if (collision.CompareTag("Player")) return;

        // code for activating InteractableObjects
        if (collision.CompareTag("LightWaveInteractable"))
        {
            BaseInteractableObject interactable = collision.GetComponent<BaseInteractableObject>();

            if (interactable != null)
            {
                interactable.Activate();
            }
        }
        // destory Projectile
        StartCoroutine(parent.DestroyLightWave());
    }
}
