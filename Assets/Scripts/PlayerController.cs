using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float movementSpeed = 5f;
    public float jumpSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = new Vector2(-movementSpeed, rb.linearVelocity.y);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = new Vector2(movementSpeed, rb.linearVelocity.y);
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
        }
    }
}
