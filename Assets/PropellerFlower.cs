using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerFlower : BaseInteractableObject
{
    [SerializeField] DesignerPlayerScriptableObject dData;
    private SpriteRenderer sr;
    private float activationTime;
    [Header("If you don't know why it's 0, keep it 0!")] 
    [SerializeField] private float maxWindStrength;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // incase this specific flowers shall have another blowing height than all the others
        if (maxWindStrength == 0) maxWindStrength = dData.maxWindStrength;
        isActive = false;
        isGlowing = false;
    }


    void Update()
    {
        if (isActive)
        {
            if (Time.time > activationTime + dData.propellerFlowerActiveTime)
            {
                this.Deactivate();
                return;
            }
            // code for hochpusten here
            
        }
    }

    public override void Activate()
    {
        this.isActive = true;
        activationTime = Time.time;
        sr.color = Color.red;
    }

    public override void Deactivate()
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
