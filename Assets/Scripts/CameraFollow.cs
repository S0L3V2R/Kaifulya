using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followObject;
    public float smoothTime = 0.2f;
    Vector3 targetPosition;
    Vector3 currentPosition;
    Vector3 Velocity = Vector3.zero;
    

    // Update is called once per frame
    void LateUpdate()
    {
        targetPosition = followObject.transform.position;
        currentPosition = transform.position;
        targetPosition[2] = -10;
        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref Velocity, smoothTime);
    }
}
