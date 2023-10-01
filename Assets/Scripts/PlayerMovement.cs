using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementTutorial : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float sensitivity = 2.0f;
    public float upDownRange = 90.0f;

    private CharacterController _characterController;
    private Camera _playerCamera;
    private float _rotationX;
    private AudioSource _steps;

    private void Awake()
    {
        _steps = GetComponent<AudioSource>();
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of the screen
        Cursor.visible = false; // Hide cursor
    }

    void Update()
    {
        // Player movement
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            _steps.enabled = true;
        }
        else
        {
            _steps.enabled = false;
        }
        var moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
        _characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Player rotation (left/right)
        var mouseX = Input.GetAxis("Mouse X") * sensitivity;
        var characterRotation = transform.localRotation.eulerAngles;
        characterRotation.y += mouseX;
        transform.localRotation = Quaternion.Euler(characterRotation);

        // Camera rotation (up/down)
        var mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -upDownRange, upDownRange);
        var cameraRotation = _playerCamera.transform.localRotation.eulerAngles;
        cameraRotation.x = _rotationX;
        _playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinArea"))
        {
            ScoreContainer.score = ScoreContainer.score * 10;
            SceneManager.LoadScene("Win");
        }
    }
}