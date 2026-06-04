using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class CameraFollow : MonoBehaviour

{

    public Transform target;          // The player

    public float smoothSpeed = 0.2f; // How smooth the camera follows

    public Vector3 offset;            // Offset from player position



    void LateUpdate()

    {

        if (target == null) return;



        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;



        // Keep camera in 2D plane

        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);

    }

}