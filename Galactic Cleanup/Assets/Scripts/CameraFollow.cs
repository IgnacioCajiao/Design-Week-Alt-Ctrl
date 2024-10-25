using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform plane;
    public Vector3 offset;
    public float smoothTime = 0.3f; 
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 targetPosition = plane.position + plane.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        transform.LookAt(plane);
    }
}