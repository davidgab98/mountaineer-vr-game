using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToTheSide : MonoBehaviour
{
    public Transform target; // VR Camera
    public Vector3 offset;

    void FixedUpdate()
    {
        // Move the snap zone inventory following the vr camera
        transform.position = target.position + Vector3.up * offset.y
            + Vector3.ProjectOnPlane(target.right, Vector3.up).normalized * offset.x
            + Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized * offset.z;

        // Rotate the snap zone by the rotation of the head (of the vr camera)
        transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
    }
}
