using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineBase : BaseInteractableObject
{
    private Transform platform;
    private Animator animator;

    private void Start()
    {
        base.Init();
        platform = this.transform.GetChild(0).GetComponent<Transform>();
        platform.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    public override void Activate()
    {
        platform.gameObject.SetActive(true);
        animator.SetBool("open", true);
        StartCoroutine(DeactivatePlatform());
    }

    protected override void Deactivate()
    {
        platform.gameObject.SetActive(false);
    }

    public void DeactivateCollision() 
    {
        this.Deactivate();
    }

    private IEnumerator DeactivatePlatform()
    {
        yield return new WaitForSeconds(dData.stayActiveTime);
        animator.SetBool("open", false);
    }
}
