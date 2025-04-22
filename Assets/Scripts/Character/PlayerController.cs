using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public Transform cam;

    public float jumpHeight = 2f;
    private bool isGrounded;

    public float sprintMultiplier = 1.5f;
    private bool isSprinting;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 moveInput;

    private PlayerInputActions inputActions;

    private bool isRightMouseHeld;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;

        inputActions.Camera.LookButton.performed += _ => isRightMouseHeld = true;
        inputActions.Camera.LookButton.canceled += _ => isRightMouseHeld = false;

        inputActions.Player.Jump.performed += _ => Jump();

        inputActions.Player.Sprint.performed += _ => isSprinting = true;
        inputActions.Player.Sprint.canceled += _ => isSprinting = false;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Update()
    {
        if (isRightMouseHeld && moveInput.y > 0)
        {
            Vector3 camForward = cam.forward;
            camForward.y = 0; // Flatten to horizontal plane
            transform.rotation = Quaternion.LookRotation(camForward);
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to stick to ground
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        bool shouldSprint = isSprinting && moveInput.y > 0;
        float currentSpeed = shouldSprint ? speed * sprintMultiplier : speed;
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
