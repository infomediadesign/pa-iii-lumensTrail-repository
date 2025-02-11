using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonsai : CollectableReceiver
{
    [SerializeField] private Transform targetTransform;
    private Vector3 targetPosition;

    private Vector3 startPosition;
    private float walkingElapsedTime = 0f;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
        targetPosition = targetTransform.position;
        targetPosition.y = transform.position.y;
    }

    protected override void ItemsDeliveredTrigger()
    {
        walkingElapsedTime += Time.deltaTime;

        float t = walkingElapsedTime / dData.bonsaiMovingTime;
        t = Mathf.Clamp01(t);

        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
