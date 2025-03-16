using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StatueWeakPoint : BaseInteractableObject
{
    StatueMainBody parent;
    Rigidbody2D lightWaveRB;
    [SerializeField] LumenThoughtBubbleActivation deactivateButtonPrompt;

    void Start()
    {
        base.Init();
        parent = GetComponentInParent<StatueMainBody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate(GameObject activationObject)
    {
        lightWaveRB = activationObject.GetComponent<Rigidbody2D>();
        parent.LightWaveHit(lightWaveRB.velocity * dData.statueVelocityMultiplier);
        deactivateButtonPrompt.SetShowPromptNow(false);
    }
}
