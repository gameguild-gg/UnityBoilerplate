using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    float velocity;

    // Stores the reference to the rigid body
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Moving Left and Right
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, rb.velocity.z);
    }
}