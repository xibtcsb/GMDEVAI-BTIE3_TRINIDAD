using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float moveSpeed = 5f;
    public float flySpeed = 10f;

    private Transform playerBody;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = transform.parent;
    }

    void Update()
    {
        // Camera Rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Player Rotation (relative to camera)
        float playerRotationY = playerBody.localRotation.eulerAngles.y;
        playerRotationY += mouseX;
        playerBody.localRotation = Quaternion.Euler(0f, playerRotationY, 0f);

        // Player Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Transform the input directions to be relative to the camera's forward and right directions
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0f; // Remove the vertical component to prevent flying and floating
        forwardDirection = forwardDirection.normalized;

        Vector3 rightDirection = transform.right;
        rightDirection.y = 0f;
        rightDirection = rightDirection.normalized;

        Vector3 moveDirection = (forwardDirection * verticalInput + rightDirection * horizontalInput).normalized;
        Vector3 moveVelocity = moveDirection * moveSpeed;
        playerBody.Translate(moveVelocity * Time.deltaTime, Space.World);

        // Fly Upwards when Shift is held
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 flyDirection = transform.up;
            Vector3 flyVelocity = flyDirection * flySpeed;
            playerBody.Translate(flyVelocity * Time.deltaTime, Space.World);
        }

        // Move Downwards when Control is held
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Vector3 flyDirection = -transform.up;
            Vector3 flyVelocity = flyDirection * flySpeed;
            playerBody.Translate(flyVelocity * Time.deltaTime, Space.World);
        }
    }
}
