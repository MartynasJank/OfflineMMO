using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class KeyBindings
    {
        public Key forward = Key.W;
        public Key backward = Key.S;
        public Key left = Key.A;
        public Key right = Key.D;
        public Key jump = Key.Space;
        public Key sprint = Key.LeftShift;
        public Key crouch = Key.LeftCtrl;
        public Key cameraToggle = Key.V;
    }

    public KeyBindings keys = new KeyBindings();

    public float speed = 5f;
    public float sprintMultiplier = 1.5f;
    public float crouchSpeedMultiplier = 0.5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public Transform cam;
    public OrbitCamera orbitCamera;

    private CharacterController controller;
    private Vector3 currentVelocity;
    private Vector3 verticalVelocity;
    private bool isGrounded;
    private bool isCrouching;
    private float originalHeight;
    private Vector3 originalCenter;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        originalCenter = controller.center;

        if (orbitCamera == null)
            orbitCamera = FindObjectOfType<OrbitCamera>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector2 moveInput = GetMovementInput(keyboard);

        if (Mouse.current != null && Mouse.current.rightButton.isPressed && moveInput.y > 0)
        {
            Vector3 camForward = cam.forward;
            camForward.y = 0;
            transform.rotation = Quaternion.LookRotation(camForward);
        }

        bool hasInput = moveInput.sqrMagnitude > 0.01f;
        bool isSprinting = keyboard[keys.sprint].isPressed && moveInput.y > 0;
        float currentSpeed = speed * (isSprinting ? sprintMultiplier : 1f) * (isCrouching ? crouchSpeedMultiplier : 1f);
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 targetVelocity = moveDirection * currentSpeed;

        float accelRate = hasInput ? acceleration : deceleration;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, accelRate * Time.deltaTime);

        if (keyboard[keys.jump].wasPressedThisFrame && isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (keyboard[keys.crouch].wasPressedThisFrame)
        {
            ToggleCrouch();
        }

        if (keyboard[keys.cameraToggle].wasPressedThisFrame && orbitCamera != null)
        {
            orbitCamera.TogglePerspective();
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        Vector3 total = currentVelocity;
        total.y = verticalVelocity.y;
        controller.Move(total * Time.deltaTime);
    }

    Vector2 GetMovementInput(Keyboard keyboard)
    {
        Vector2 input = Vector2.zero;
        if (keyboard[keys.forward].isPressed) input.y += 1f;
        if (keyboard[keys.backward].isPressed) input.y -= 1f;
        if (keyboard[keys.left].isPressed) input.x -= 1f;
        if (keyboard[keys.right].isPressed) input.x += 1f;
        return Vector2.ClampMagnitude(input, 1f);
    }

    void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        controller.height = isCrouching ? originalHeight * 0.5f : originalHeight;
        controller.center = isCrouching ? originalCenter * 0.5f : originalCenter;
    }
}

