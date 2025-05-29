using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SmoothFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform targetToFollow;
    [SerializeField] private float followSpeed = 15;

    void Update()
    {
        if (targetToFollow == null) return;

        Vector3 desiredPosition = targetToFollow.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
