using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public float speed = 15f;          // Forward/Backward speed
    public float turnSpeed = 50f;      // Turning speed
    public float brakeStrength = 10f;  // How fast the car brakes

    private Rigidbody rb;

    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Get player input
        float moveInput = Input.GetAxis("Vertical");   // Forward/Backward
        float turnInput = Input.GetAxis("Horizontal"); // Left/Right

        // Apply movement in the direction the car is facing
        Vector3 move = transform.forward * moveInput * speed;  // Use transform.forward for the car's local forward direction
        rb.MovePosition(rb.position + move * Time.deltaTime);

        // Apply turning based on the Y-axis
        float turn = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);

        // Braking mechanism
        if (Input.GetKey(KeyCode.Space)) // Press space to brake
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, brakeStrength * Time.deltaTime);
        }
    }
}
