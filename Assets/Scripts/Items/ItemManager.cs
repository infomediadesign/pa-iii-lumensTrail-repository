using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public GameObject carriedItem = null;
    private int targetLayer = 13;
    
    // Start is called before the first frame update
    void Start()
    {
        // Find all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Filter objects by their layer
        GameObject[] objectsInLayer = allObjects
            .Where(obj => obj.layer == targetLayer)
            .ToArray();

        // Log or process objects
        foreach (GameObject obj in objectsInLayer)
        {
            Debug.Log("Found object in layer: " + obj.name);
        }
        items.AddRange(objectsInLayer);
        return;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetNearestPickupItem(Transform playerPos, float pickupRadius, bool facingRight, ref bool pickup)
    {
        pickup = true;
        GameObject returnItem = null;
        float returnDistance = pickupRadius;
        this.UpdateItemList();
        foreach (var item in items)
        {
            if (Vector2.Distance(item.transform.position, playerPos.position) > pickupRadius) continue;

            if (facingRight)
            {
                if (item.transform.position.x >= playerPos.position.x)
                {
                    if (returnItem == null || (Mathf.Abs(returnDistance) > Mathf.Abs(item.transform.position.x - playerPos.position.x))) returnItem = item;
                }
            } 
            else
            {
                if (item.transform.position.x <= playerPos.position.x)
                {
                    if (returnItem == null || (Mathf.Abs(returnDistance) > Mathf.Abs(item.transform.position.x - playerPos.position.x))) returnItem = item;
                }
            }
        }
        if (returnItem == null) pickup = false;
        return returnItem;
    }

    public void UpdateItemList() 
    {
        for (int i = 0; i < items.Count; i++) 
        {
            if (items[i] == null) {
                items.RemoveAt(i);
            }
        }
    }
}
