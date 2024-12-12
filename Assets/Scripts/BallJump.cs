using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallJump : MonoBehaviour
{
    public Vector3 jump;
    public float jumpForce = 4.0f;
    public float gravity = -9.81f;
    public float gravityScale = 1;
      
    public bool isGrounded;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        { 
            rb.AddForce(jumpForce * jump, ForceMode.Impulse);
            isGrounded = false;
        }
    }
      
    void OnCollisionStay()
    {
        isGrounded = true;
    }
    
    void OnCollisionExit()
    {
        isGrounded = false;
    }
}
