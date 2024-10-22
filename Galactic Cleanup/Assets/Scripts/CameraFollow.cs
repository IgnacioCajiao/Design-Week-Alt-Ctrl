using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform plane;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = plane.position + offset;

        transform.LookAt(plane);
    }
}

