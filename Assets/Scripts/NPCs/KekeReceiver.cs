using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KekeReceiver : CollectableReceiver
{
    public Animator kekeAnimator;
    public GameObject chaseExecution;

    [SerializeField] private GameObject chaseSprite;

    public ItemManager itemManager;
    public KekeAIAdvanced kekeAI;

    public LumenThoughtBubbleActivation lumenThoughtBubbleActivation;
    
    protected override void Awake()
    {
        base.Awake();
        kekeAI = GetComponent<KekeAIAdvanced>();
        kekeAI.OnPathfindingDisable();
    }

    private new void Update()
    {
        base.Update();
    }

    protected override void ItemsDeliveredTrigger()
    {
        kekeAnimator.SetTrigger("standup");
        itemManager.items = null;
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        kekeAnimator.SetTrigger("eating");
    }

    public void OnStanding()
    {
        chaseExecution.SetActive(true);
        chaseSprite.SetActive(true);
        kekeAnimator.gameObject.SetActive(false);
    }
}
