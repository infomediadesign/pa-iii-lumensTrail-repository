using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BaseInteractableObject : MonoBehaviour
{
    [SerializeField] protected DesignerPlayerScriptableObject dData;

    [SerializeField] protected ParticleSystem ps;
    [SerializeField] protected Light2D spotLight;
    private float glowLightIntensity;
    protected bool isActive;
    protected bool isGlowing;
    protected SpriteRenderer sr;
    protected float glowOnTime;
    void Start()
    {
        
    }

    protected void Init()
    {
        this.sr = GetComponent<SpriteRenderer>();
        this.glowLightIntensity = dData.highlightLightIntensity;
        this.isActive = false;
        this.isGlowing = false;
        this.spotLight.intensity = 0f;
    }

    void Update()
    {
        
    }

    IEnumerator FadeEffect(float startValue, float endValue, float duration) 
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            this.spotLight.intensity = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }
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
            StartCoroutine(FadeEffect(0f, glowLightIntensity, 1f));
            this.isGlowing = true;
            this.glowOnTime = Time.time;
            //var main = ps.main;
            //main.duration = dData.highlightTime;
            //ps.Play();
            this.StartCoroutine(Glowing());
        }
    }

    protected virtual void GlowOff()
    {
        this.isGlowing = false;
        StartCoroutine(FadeEffect(glowLightIntensity, 0f, 1f));
        //ps.Stop();
    }

    protected IEnumerator Glowing()
    {
        yield return new WaitForSeconds(dData.highlightTime+1);
        this.GlowOff();
    }

}
