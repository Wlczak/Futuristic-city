using UnityEngine;
using System.Collections;

public class MyPlayerController2 : MonoBehaviour
{
    CharacterController characterController;

    public GameObject _camera;          // Player camera
    public GameObject car;              // Car object to follow
    public float _speed = 6.0f;
    public float _runSpeed = 12.0f;
    public float _jumpSpeed = 8.0f;
    public float _rotationSpeed = 10.0f;
    public float _gravity = 20.0f;
    public KeyCode _run;
    public KeyCode _flyDown;
    public KeyCode switchKey = KeyCode.C;  // Key to switch between player and car follow

    private Vector3 moveDirection = Vector3.zero;

    // Player angles in degrees
    private float pitch;
    private float yaw;

    private bool isFollowingCar = false;  // Tracks whether we're in car-following mode

    // Camera offset for following the car
    public Vector3 carCameraOffset = new Vector3(-510, 5, -10); // Adjust values as needed

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        yaw = this.transform.localEulerAngles.y;
        pitch = _camera.transform.localEulerAngles.x; // Changed to X for camera pitch
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Check for camera switch input
        if (Input.GetKeyDown(switchKey))
        {
            isFollowingCar = !isFollowingCar;

            if (!isFollowingCar)
            {
                // Reset camera position to the player when switching back
                _camera.transform.position = this.transform.position + Vector3.up * 1.5f; // Adjust the height as needed
                _camera.transform.rotation = Quaternion.Euler(pitch, yaw, 0); // Reset camera rotation to match player view
            }
        }

        if (isFollowingCar)
        {
            // If in car-following mode, move the camera behind the car
            FollowCar();
        }
        else
        {
            // Otherwise, control the player
            ControlPlayer();
        }
    }

    // Handles player control
    private void ControlPlayer()
    {
        pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * _rotationSpeed, -90, 90);
        yaw += Input.GetAxis("Mouse X") * _rotationSpeed;

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
        if (Input.GetKey(_flyDown))
        {
            moveDirection.y = -_jumpSpeed;
        }

        this.transform.localEulerAngles = new Vector3(0, yaw, 0);
        moveDirection = this.transform.TransformDirection(moveDirection);

        _camera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        // Apply gravity
        moveDirection.y -= _gravity * Time.deltaTime;

        // Move the player controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    // Handles following the car
    private void FollowCar()
    {
        // Get the car's forward direction
        Vector3 carForward = car.transform.forward;

        // Calculate the desired camera position based on the car's position, its forward direction, and the offset
        Vector3 desiredPosition = car.transform.position - carForward * -carCameraOffset.z + Vector3.up * carCameraOffset.y + Vector3.forward * carCameraOffset.x;

        // Set the camera position to the desired position
        _camera.transform.position = desiredPosition;

        // Make the camera look at the car
        _camera.transform.LookAt(car.transform.position + Vector3.up * 1.5f); // Look at the car's position at a certain height
    }

}
