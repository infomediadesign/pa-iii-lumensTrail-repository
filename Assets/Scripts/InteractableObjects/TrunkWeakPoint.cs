using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkWeakPoint : BaseInteractableObject
{
    TrunkMainBody parent;
    Rigidbody2D lightWaveRB;

    void Start()
    {
        base.Init();
        parent = GetComponentInParent<TrunkMainBody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate(GameObject activationObject)
    {
        lightWaveRB = activationObject.GetComponent<Rigidbody2D>();
        parent.LightWaveHit(lightWaveRB.velocity * dData.trunkVelocityMultiplier);
        this.gameObject.SetActive(false);
    }
}
