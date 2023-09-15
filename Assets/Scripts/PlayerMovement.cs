using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementTutorial : MonoBehaviour
{
    public float moveSpeed = 5.0f;
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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Player rotation (left/right)
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        Vector3 characterRotation = transform.localRotation.eulerAngles;
        characterRotation.y += mouseX;
        transform.localRotation = Quaternion.Euler(characterRotation);

        // Camera rotation (up/down)
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -upDownRange, upDownRange);
        Vector3 cameraRotation = playerCamera.transform.localRotation.eulerAngles;
        cameraRotation.x = rotationX;
        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation);
    }
}