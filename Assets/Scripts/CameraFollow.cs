using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow; // The object you want to follow
    public float offsetZ = -10f; // Z-axis offset for the camera position
    
    private Rigidbody rb;  // Rigidbody reference
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component
    }
    
    // Update is called once per frame
    void Update()
    {
         // Calculate the new position for the camera (keeping Y and Z fixed)
        Vector3 targetPosition = new Vector3(objectToFollow.position.x, objectToFollow.position.y, offsetZ);

        // Move the camera smoothly to the target position
        rb.MovePosition(targetPosition);
    }
}