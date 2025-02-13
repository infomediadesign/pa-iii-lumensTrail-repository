using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LumenThoughtBubble : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer childSR;
    private float targetAlpha;    
    private float fadeDuration = 1f; 
    private float fadeTime;          
    private bool isFading = false;

    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 0;
        this.spriteRenderer.color = color;
        this.childSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        this.transform.position = target.transform.position + offset;   

        if (isFading)
        {
            fadeTime += Time.deltaTime;
            float currentAlpha = spriteRenderer.color.a;
            float alpha = Mathf.Lerp(currentAlpha, targetAlpha, fadeTime / fadeDuration);

            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;

            if (Mathf.Abs(color.a - targetAlpha) < 0.01f)
            {
                color.a = targetAlpha;
                spriteRenderer.color = color;
                isFading = false;
            }
        }
    }

    public void SetButtonSprite(Sprite input) 
    {
        this.childSR.sprite = input;
    }

    public void StartFade(float newTargetAlpha, float duration)
    {
        targetAlpha = newTargetAlpha;
        fadeDuration = duration;
        fadeTime = 0f;
        isFading = true;
    }

    public float GetCurrentAlpha()
    {
        return this.spriteRenderer.color.a;
    }
}
