using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CollectableReceiver : MonoBehaviour
{
    [SerializeField] private GameObject items;
    private GameObject carriedObject;
    private int totalItems;
    private int deliveredItems;
    private bool delivering = false;

    private float elapsedTime = 0;
    private float duration = 1f;
    private float arcHeight = 1f;
    private Vector2 startPoint;
    private Vector2 endPoint;

    [SerializeField] protected DesignerPlayerScriptableObject dData;

    

    protected virtual void Start()
    {
        totalItems = items.transform.childCount;
        deliveredItems = 0;
        this.endPoint = this.transform.position;
    }

    protected virtual void Update()
    {
        if (delivering)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            Vector2 currentPosition = CalculateArcPosition(startPoint, endPoint, arcHeight, t);
            carriedObject.transform.position = currentPosition;

            if (t >= 1)
            {
                carriedObject.transform.position = endPoint;
            }
        }

        //

        if (deliveredItems == totalItems)
        {
            this.ItemsDeliveredTrigger();
        }
    }

    protected virtual void OnBecameInvisible()
    {
        if (deliveredItems != totalItems) return;
        Destroy(gameObject);
    }

    protected virtual void ItemsDeliveredTrigger()
    {
        
    }

    public virtual void DeliverItem(GameObject carriedItem)
    {
        this.elapsedTime = 0;
        this.carriedObject = carriedItem;
        this.startPoint = carriedObject.transform.position;
        this.delivering = true;
    }

    private Vector2 CalculateArcPosition(Vector2 start, Vector2 end, float arcHeight, float t)
    {
        // Linear interpolation between start and end
        Vector2 linearPoint = Vector2.Lerp(start, end, t);

        // Add the parabolic arc offset to the Y component
        float arc = 4 * arcHeight * (t - t * t); // Parabola: h(t) = 4h * (t - t^2)
        linearPoint.y += arc;

        return linearPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == carriedObject && delivering)
        {
            carriedObject.GetComponent<Collider2D>().enabled = false;
            carriedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            carriedObject.GetComponent<SpriteRenderer>().enabled = false;
            carriedObject.transform.position = endPoint;
            this.delivering = false;
            this.deliveredItems++;
        }
    }
}
