using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float cameraSpeed = 1f;
    public Vector3 cameraOffset;
    public float maxDistance = 6f;
    public float followPlayerAtScreenPercentage = 80f;
    private Camera cam;
    private bool playerPos = false;
    private bool stopFollowing = false;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    /*
    private void LateUpdate()
    { 
        Vector3 desiredPosition = target.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed * Time.deltaTime);

        if (Vector3.Distance(smoothedPosition, target.position) > maxDistance)
        {
            Vector3 direction = (smoothedPosition - target.position).normalized;
            smoothedPosition = target.position + direction * maxDistance;
        }
        transform.position = smoothedPosition;
    }
    */

    private void LateUpdate()
    {
        if (stopFollowing) 
        {
            Vector3 playerViewportPos = cam.WorldToViewportPoint(target.position);
            bool isInCorrectHalf = playerViewportPos.x > 0.5f;
            if (isInCorrectHalf) stopFollowing = false;
        }

        float desiredYPos;
        if (target.transform.position.y < cam.orthographicSize * (followPlayerAtScreenPercentage / 100))
        {
            desiredYPos = cameraOffset.y;
            playerPos = false;
        }
        else
        {
            desiredYPos = target.transform.position.y;
            playerPos = true;

        }
        Vector3 desiredPosition = new Vector3(target.position.x + cameraOffset.x, desiredYPos, cameraOffset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed * Time.deltaTime);

        if (playerPos)
        {
            if (Vector3.Distance(smoothedPosition, target.position) > maxDistance)
            {
                Vector3 direction = (smoothedPosition - target.position).normalized;
                smoothedPosition = target.position + direction * maxDistance;
            }
        }

        if (stopFollowing) smoothedPosition.x = transform.position.x;
        transform.position = smoothedPosition;
    }

    public void SetStopFollowing(bool input)
    {
        this.stopFollowing = input;
    }
}
 