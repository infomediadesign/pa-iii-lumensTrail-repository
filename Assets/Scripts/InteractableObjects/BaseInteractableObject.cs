using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractableObject : MonoBehaviour
{
    [SerializeField] protected DesignerPlayerScriptableObject dData;
    protected bool isActive;
    protected bool isGlowing;
    protected SpriteRenderer sr;
    protected float glowOnTime;
    protected Color orginColor;
    void Start()
    {
        
    }

    protected void Init()
    {
        this.sr = GetComponent<SpriteRenderer>();
        this.isActive = false;
        this.isGlowing = false;
    }

    void Update()
    {
        
    }

    public virtual void Activate()
    {

    }

    public virtual void Activate(GameObject activationObject)
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
            this.StartCoroutine(Glowing());
        }
    }

    protected virtual void GlowOff()
    {
        this.isGlowing = false;
        this.sr.color = orginColor;
    }

    protected IEnumerator Glowing()
    {
        yield return new WaitForSeconds(dData.highlightTime);
        this.GlowOff();
    }

}
