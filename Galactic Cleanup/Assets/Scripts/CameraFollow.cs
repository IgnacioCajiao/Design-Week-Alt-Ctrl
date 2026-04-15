using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform plane;
    public Vector3 offset;
    public Vector3 lookOffset;   // New
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (plane == null) return;

        Vector3 targetPosition = plane.position + plane.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        transform.LookAt(plane.position + plane.TransformDirection(lookOffset));
    }
}