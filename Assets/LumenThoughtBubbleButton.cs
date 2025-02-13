using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumenThoughtBubbleButton : MonoBehaviour
{
    private LumenThoughtBubble parent;
    private SpriteRenderer sr;

    void Start()
    {
        parent = GetComponentInParent<LumenThoughtBubble>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        this.SetCurrentAlpha();
    }

    private void SetCurrentAlpha()
    {
        Color color = this.sr.color;
        color.a = parent.GetCurrentAlpha();
        this.sr.color = color;
    }
}
