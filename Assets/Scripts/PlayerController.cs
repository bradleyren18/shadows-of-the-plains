using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    // Components
    private CharacterController controller;
    private Camera playerCamera;
    private PlayerInput playerInput;

    // Movement variables
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;

    // Input variables
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool runPressed;

    // Mouse look variables
    private float verticalRotation = 0f;

    // block placement variables
    [SerializeField]
    private GameObject cubePrefab;

    // hand variables
    [SerializeField]
    private GameObject hand1;
    [SerializeField]
    private GameObject hand2;

    void Start()
    {
        // Get components
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        playerInput = GetComponent<PlayerInput>();

        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            TryPlaceBlock();
        }
    }

    // Input callback methods - called automatically by PlayerInput component
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleMouseLook()
    {
        // Apply mouse sensitivity
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        // Check if grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }

        // Determine speed (walk/run)
        currentSpeed = runPressed ? runSpeed : walkSpeed;

        // Calculate movement direction
        Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Apply movement
        controller.Move(direction * currentSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void TryPlaceBlock()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        RotateHand(hand2);

        // Raycast to detect block within range
        if (Physics.Raycast(ray, out hit, 4))
        {
            // Must be hitting an existing block
            if (hit.collider.name.StartsWith("grass"))
            {
                // Calculate placement position: hit point + normal vector
                Vector3 placePos = hit.collider.transform.position + hit.normal;

                // Snap position to nearest whole number
                placePos = new Vector3(
                    Mathf.Round(placePos.x),
                    Mathf.Round(placePos.y),
                    Mathf.Round(placePos.z)
                );

                // Check if space is empty
                if (!Physics.CheckBox(placePos, Vector3.one * 0.45f))
                {
                    Instantiate(cubePrefab, placePos, Quaternion.identity);
                }
            }
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // Re-lock cursor when window regains focus
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void RotateHand(GameObject hand)
    {
        StartCoroutine(rotateHand(hand));
    }

    IEnumerator rotateHand(GameObject hand)
    {
        hand.transform.Rotate(30, 0, 0);
        yield return new WaitForSeconds(0.25f); // Wait 1 second
        hand.transform.Rotate(-30, 0, 0);
    }
}
