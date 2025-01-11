using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerFlower : BaseInteractableObject
{
    [SerializeField] DesignerPlayerScriptableObject dData;
    private float activationTime;
    [Header("If you don't know why they're 0, keep em 0!")] 
    [SerializeField] private float maxWindStrength;
    [SerializeField] private float propellerFlowerActiveTime;

    void Start()
    {
        this.sr = GetComponent<SpriteRenderer>();
        // incase this specific flowers shall have another blowing height than all the others
        if (maxWindStrength == 0) maxWindStrength = dData.maxWindStrength;
        if (propellerFlowerActiveTime == 0) propellerFlowerActiveTime = dData.propellerFlowerActiveTime;
        isActive = false;
        isGlowing = false;
    }


    void Update()
    {
        if (isActive)
        {
            if (Time.time > activationTime + propellerFlowerActiveTime)
            {
                this.Deactivate();
                return;
            }
        }

        if (isGlowing)
        {
            if (Time.time > this.glowOnTime + dData.highlightTime)
            {
                this.GlowOff(); 
                return;
            }
        }
    }

    public override void Activate()
    {
        if (isActive) return;
        this.isActive = true;
        activationTime = Time.time;
        sr.color = Color.red;
    }

    protected override void Deactivate()
    {
        this.isActive = false;
        sr.color = Color.white;
    }

    public void OnPlayerStayWindTrigger(Collider2D collision)
    {
        if (!this.isActive) return;
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // upward force
                playerRb.AddForce(Vector2.up * maxWindStrength, ForceMode2D.Force);
            }
        }
    }
}
