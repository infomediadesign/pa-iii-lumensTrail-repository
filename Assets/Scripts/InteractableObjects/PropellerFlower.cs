using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerFlower : BaseInteractableObject
{
    private float activationTime;
    [Header("If you don't know why they're 0, keep em 0!")] 
    [SerializeField] private float maxWindStrength;
    [SerializeField] private float propellerFlowerActiveTime;
    private Animator animator;

    void Start()
    {
        base.Init();
        animator = GetComponent<Animator>();
        // incase this specific flowers shall have another blowing height than all the others
        if (maxWindStrength == 0) maxWindStrength = dData.maxWindStrength;
        if (propellerFlowerActiveTime == 0) propellerFlowerActiveTime = dData.propellerFlowerActiveTime;
    }


    void Update()
    {
        
    }

    public override void Activate()
    {
        if (isActive) return;
        this.isActive = true;
        animator.SetBool("active", true);
        activationTime = Time.time;
        this.StartCoroutine(PropellerFlowerActive());
    }

    protected override void Deactivate()
    {
        this.isActive = false;
        animator.SetBool("active", false);
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

    private IEnumerator PropellerFlowerActive()
    {
        yield return new WaitForSeconds(dData.propellerFlowerActiveTime);
        this.Deactivate();
    }
}
