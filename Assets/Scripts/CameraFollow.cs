using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float cameraSpeed = 1;
    public Vector3 cameraOffset;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + cameraOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed * Time.deltaTime);
    }

}
 