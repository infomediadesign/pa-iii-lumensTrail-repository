using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractableObject : MonoBehaviour
{
    protected bool isActive;
    protected bool isGlowing;
    protected SpriteRenderer sr;
    protected float glowOnTime;
    protected Color orginColor;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void Activate()
    {

    }

    protected virtual void Deactivate()
    {

    }

    public virtual void GlowOn()
    {
        if (!isGlowing)
        {
            this.isGlowing = true;
            this.glowOnTime = Time.time;
            this.orginColor = this.sr.color;
            this.sr.color = Color.yellow;
        }
    }

    protected virtual void GlowOff()
    {
        this.isGlowing = false;
        this.sr.color = orginColor;
    }

}
