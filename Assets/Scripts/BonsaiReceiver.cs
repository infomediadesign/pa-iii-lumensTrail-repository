using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonsaiReceiver : CollectableReceiver
{
    [SerializeField] private Transform targetTransform;
    private Vector3 targetPosition;
    private Transform player;
    private Vector3 startPosition;
    private float walkingElapsedTime = 0f;
    private bool isWalking = false;
    private bool hasPatted = false;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        startPosition = transform.position;
        this.animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>().gameObject.transform;
    }

    private new void Update()
    {
        base.Update();
        /*
        if (isWalking)
        {
            walkingElapsedTime += Time.deltaTime;

            float t = walkingElapsedTime / dData.bonsaiMovingTime;
            t = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1f)
            {
                isWalking = false;
                //this.DestroyReceiver();
            }
        }
        */
        if (isWalking)
        {
            Vector3 direction = (targetPosition - startPosition).normalized;

            float step = dData.bonsaiMovingSpeed * Time.deltaTime;

            transform.position += direction * step;

            if (Vector3.Distance(transform.position, targetPosition) <= (hasPatted? step : dData.bonsaiLumenStandingDistance))
            {
                isWalking = false;
                if (!hasPatted) 
                {
                    hasPatted = true;
                    animator.SetBool("patting", true);
                }
                else this.DestroyReceiver();
            }
            else Debug.Log(Vector3.Distance(transform.position, targetPosition));
        }
    }

    protected override void ItemsDeliveredTrigger()
    {
        animator.SetBool("standup", true);
        MovementBaseState.movementEnabled = false;
        this.GetComponent<Collider2D>().enabled = false;
    }

    private void OnBecameInvisible()
    {
        if (isWalking) this.DestroyReceiver();
    }

    public void SetPattingPos() 
    {
        targetPosition = player.transform.position;
        targetPosition.y = transform.position.y;
        this.isWalking = true;
        this.animator.SetBool("walking", true);
    }

    public void SetEndPos() 
    {
        Debug.Log("Hallo");
        targetPosition = targetTransform.position;
        targetPosition.y = transform.position.y;
        this.isWalking = true;
        this.animator.SetBool("patting", false);
        MovementBaseState.movementEnabled = true;
    }
}