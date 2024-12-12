using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float cameraSpeed = 1f;
    public Vector3 cameraOffset;
    public float maxDistance = 6f;

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
}
 