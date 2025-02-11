using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonsaiThoughtBubble : MonoBehaviour
{
    private CollectableReceiver parent;

    private SpriteRenderer spriteRenderer;
    private float targetAlpha;       
    private float fadeDuration = 1f; 
    private float fadeTime;          
    private bool isFading = false;
    [SerializeField] private DesignerPlayerScriptableObject dData;
    [SerializeField] private ProgrammerPlayerScriptableObject pData;

    void Awake()
    {
        parent = GetComponentInParent<CollectableReceiver>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
    }

    void Update()
    {
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

    private void StartFade(float newTargetAlpha, float duration)
    {
        targetAlpha = newTargetAlpha;
        fadeDuration = duration;
        fadeTime = 0f;
        isFading = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartFade(1f, dData.thoughtBubbleFadeTime);
            pData.inDropRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartFade(0f, dData.thoughtBubbleFadeTime);
            pData.inDropRange = false;
        } 
    }

    public float GetCurrentAlpha()
    {
        return this.spriteRenderer.color.a;
    }
}
