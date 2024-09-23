using UnityEngine;
using System.Collections;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class MyPlayerController2 : MonoBehaviour
{
    CharacterController characterController;

    public GameObject _camera;
    public float _speed = 6.0f;
    public float _runSpeed = 12.0f;    
    public float _jumpSpeed = 8.0f;
    public float _rotationSpeed = 10.0f;
    public float _gravity = 20.0f;
    public KeyCode _run;

    private Vector3 moveDirection = Vector3.zero;

    // Player angles in degrees
    private float pitch;
    private float yaw;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        yaw = this.transform.localEulerAngles.y;
        pitch = _camera.transform.localEulerAngles.z;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * _rotationSpeed, -90, 90);

        yaw += Input.GetAxis("Mouse X") * _rotationSpeed;

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            float speed = _speed;

            if (Input.GetKey(_run)) 
            {
                speed = _runSpeed;
            }

            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = _jumpSpeed;
            }
            
            this.transform.localEulerAngles = new Vector3(0, yaw, 0);
            
            moveDirection = this.transform.TransformDirection(moveDirection);            
        }
        
        this.transform.localEulerAngles = new Vector3(0, yaw, 0);
        _camera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= _gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        
    }
}