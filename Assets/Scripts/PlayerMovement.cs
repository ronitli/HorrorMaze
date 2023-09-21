using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTutorial : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float sensitivity = 2.0f;
    public float upDownRange = 90.0f;

    private CharacterController characterController;
    private Camera playerCamera;
    private float rotationX = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of the screen
        Cursor.visible = false; // Hide cursor
    }

    void Update()
    {
        // Player movement
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        var moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Player rotation (left/right)
        var mouseX = Input.GetAxis("Mouse X") * sensitivity;
        var characterRotation = transform.localRotation.eulerAngles;
        characterRotation.y += mouseX;
        transform.localRotation = Quaternion.Euler(characterRotation);

        // Camera rotation (up/down)
        var mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -upDownRange, upDownRange);
        var cameraRotation = playerCamera.transform.localRotation.eulerAngles;
        cameraRotation.x = rotationX;
        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WinArea")
        {
            Debug.Log("win");
        }
        else if (other.tag == "Enemy")
        {
            Debug.Log("GG");
        }
    }
}