using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonsai : CollectableReceiver
{
    [SerializeField] private Transform targetTransform;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float walkingElapsedTime = 0f;
    private bool isWalking = false;

    protected override void Awake()
    {
        base.Awake();
        startPosition = transform.position;
        targetPosition = targetTransform.position;
        targetPosition.y = transform.position.y;
    }

    private new void Update()
    {
        base.Update();

        if (isWalking)
        {
            walkingElapsedTime += Time.deltaTime;

            float t = walkingElapsedTime / dData.bonsaiMovingTime;
            t = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1f)
            {
                this.DestroyReceiver();
            }
        }
    }

    protected override void ItemsDeliveredTrigger()
    {
        isWalking = true;
        this.GetComponent<Collider2D>().enabled = false;
    }

    private void OnBecameInvisible()
    {
        if (isWalking) this.DestroyReceiver();

    }
}