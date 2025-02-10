using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bonsai : MonoBehaviour
{
    [SerializeField] private PlayerController pC;
    [SerializeField] private GameObject item01;
    [SerializeField] private GameObject item02;
    private bool secondItemDelivered = false;
    private bool allItemsDelivered = false;
    

    [SerializeField] private SpriteRenderer desItem;
    [SerializeField] private DesignerPlayerScriptableObject dData;

    private GameObject currentDesiredItem;

    [SerializeField] private Transform targetTransform;
    private Vector3 targetPosition;

    private Vector3 startPosition;
    private float elapsedTime = 0f;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = targetTransform.position;
        targetPosition.y = transform.position.y;
        currentDesiredItem = item01;
        desItem.sprite = currentDesiredItem.GetComponent<SpriteRenderer>().sprite;
        desItem.color = currentDesiredItem.GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {
        if (allItemsDelivered)
        {
            {
                elapsedTime += Time.deltaTime;

                float t = elapsedTime / dData.bonsaiMovingTime;
                t = Mathf.Clamp01(t); 

                transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                if (t >= 1f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public GameObject GetCurrentDesiredItem()
    {
        return this.currentDesiredItem;
    }

    public void ItemAccepted()
    {
        pC.TriggerPickupManually();
        if (!secondItemDelivered)
        {
            secondItemDelivered = true;
            this.DisableCurrentDesiredItem();
            currentDesiredItem = item02;
            desItem.sprite = currentDesiredItem.GetComponent<SpriteRenderer>().sprite;
            desItem.color = currentDesiredItem.GetComponent<SpriteRenderer>().color;
        }
        else
        {
            this.DisableCurrentDesiredItem();
            allItemsDelivered = true;
            this.GetComponent<Collider2D>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            Destroy(item01.gameObject);
            Destroy(item02.gameObject);
        }
    }

    private void DisableCurrentDesiredItem()
    {
        currentDesiredItem.transform.position = this.transform.position;
        currentDesiredItem.GetComponent<SpriteRenderer>().enabled = false;
        currentDesiredItem.GetComponent<Collider2D>().enabled = false;
    }

    private void OnBecameInvisible()
    {
        if (!allItemsDelivered) return;
        Destroy(gameObject);
    }
}
