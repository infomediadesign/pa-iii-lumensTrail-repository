using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VinesPlatform : BaseInteractableObject
{
    private Transform childTransform;

    void Start()
    {
        base.Init();
        this.childTransform = this.gameObject.transform.GetChild(0);
        if (this.childTransform == null) Debug.Log("children transform null");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate()
    {
        if (this.isActive) return;
        this.isActive = true;
        this.ExpandVines();
    }

    protected override void Deactivate()
    {
        this.isActive = false;
    }

    private void ExpandVines()
    {
        this.childTransform.localScale = new Vector3(this.childTransform.localScale.x + dData.expandDistance / 2, this.childTransform.localScale.y, 0);
        this.childTransform.position = new Vector3(this.childTransform.position.x + dData.expandDistance / 2, this.childTransform.position.y, 0);
        StartCoroutine(VinesExpandedTimer());
    }

    private void ReturnVines()
    {
        this.childTransform.localScale = new Vector3(this.childTransform.localScale.x - dData.expandDistance / 2, this.childTransform.localScale.y, 0);
        this.childTransform.position = new Vector3(this.childTransform.position.x - dData.expandDistance / 2, this.childTransform.position.y, 0);
        this.Deactivate();
    }

    private IEnumerator VinesExpandedTimer()
    {
        yield return new WaitForSeconds(dData.stayActiveTime);
        ReturnVines();
    }
}
