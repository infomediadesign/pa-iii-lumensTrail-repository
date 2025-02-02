using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class VineBase : BaseInteractableObject
{
    private Transform platform;

    private void Start()
    {
        base.Init();
        platform = this.transform.GetChild(0).GetComponent<Transform>();
        platform.gameObject.SetActive(false);
    }

    public override void Activate()
    {
        platform.gameObject.SetActive(true);
        StartCoroutine(DeactivatePlatform());
    }

    protected override void Deactivate()
    {
        platform.gameObject.SetActive(false);
    }

    private IEnumerator DeactivatePlatform()
    {
        yield return new WaitForSeconds(dData.stayActiveTime);
        this.Deactivate();
    }
}
