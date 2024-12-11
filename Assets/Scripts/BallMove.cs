using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    public float ForceStreght = 5f;
    // public float moveSpeed = 5f;
    // public float jumpForce = 10f;
    // private bool jump = false;
    // private float ySpeed;
    //
    // public Transform groundCheck;
    // public bool grounded = false;

    // Stores the reference to the rigid body
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // This is not a Jump, need to implement a logic regarding idGounded
        if (Input.GetKey(KeyCode.W))
            rb.velocity += Time.deltaTime * ForceStreght * Vector3.up; //
        if (Input.GetKey(KeyCode.D))
            rb.velocity += Time.deltaTime * ForceStreght * Vector3.right;
        if (Input.GetKey(KeyCode.A))
            rb.velocity += Time.deltaTime * ForceStreght * Vector3.left;
    }
}

// Update is called once per frame
//     void Update()
//     {
//         if (Input.GetKey(KeyCode.W))
//             rb.velocity += new Vector3(horizontalInput * moveSpeed, verticalInput, rb.velocity.z); //
//         if (Input.GetKey(KeyCode.D))
//             rb.velocity +=  Time.deltaTime * ForceStreght * Vector3.right;
//         if (Input.GetKey(KeyCode.A))
//             rb.velocity +=  Time.deltaTime * ForceStreght * Vector3.left;
//         
//         float horizontalInput = Input.GetAxis("Horizontal");
//         float verticalInput = Input.GetAxis("Vertical");
//         rb.velocity = new Vector3(horizontalInput * moveSpeed, verticalInput, rb.velocity.z);
//         
//         ySpeed =+ Physics.gravity.y * Time.deltaTime;
//         
//         if (Input.GetButtonDown("Jump") && grounded)
//             
//         
//     
//     // Check if the player is grounded (to prevent double jumps)
//     void OnCollisionStay(Collision other)
//     {
//         if (other.gameObject.CompareTag("Ground"))
//         {
//             isGrounded = true;
//         }
//     }
//
//     void OnCollisionExit(Collision other)
//     {
//         if (other.gameObject.CompareTag("Ground"))
//         {
//             isGrounded = false;
//         }
//     }
// }

/*
public class BallMove : MonoBehaviour
{
    public float ForceStreght = 1.0f;
    // Stores the reference to the rigid body
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // This is not a Jump, need to implement a logic regarding idGounded
        if (Input.GetKey(KeyCode.W))
            rb.velocity += Time.deltaTime * ForceStreght * Vector3.up; //
        if (Input.GetKey(KeyCode.D))
            rb.velocity +=  Time.deltaTime * ForceStreght * Vector3.right;
        if (Input.GetKey(KeyCode.A))
            rb.velocity +=  Time.deltaTime * ForceStreght * Vector3.left;
    }
}
 */
