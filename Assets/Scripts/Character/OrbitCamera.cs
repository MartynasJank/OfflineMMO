using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    public Transform target; // Player
    public Transform cameraTransform; // Main Camera
    public float distance = 4f;
    public float xSpeed = 120f;
    public float ySpeed = 80f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float firstPersonHeight = 1.6f;
    public float collisionRadius = 0.2f;
    public LayerMask collisionLayers = ~0;

    private float x = 0f;
    private float y = 0f;
    private Vector2 lookInput;
    private bool isFirstPerson;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Camera.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Camera.Look.canceled += _ => lookInput = Vector2.zero;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    public void TogglePerspective()
    {
        isFirstPerson = !isFirstPerson;
    }

    void LateUpdate()
    {
        if (!isFirstPerson && Mouse.current != null)
        {
            float scrollValue = Mouse.current.scroll.ReadValue().y;
            distance -= scrollValue * 0.5f;
            distance = Mathf.Clamp(distance, 2f, 20f);
        }

        if (Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            x += lookInput.x * xSpeed * Time.deltaTime;
            y -= lookInput.y * ySpeed * Time.deltaTime;
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;

        Vector3 targetPos = target.position + Vector3.up * firstPersonHeight;

        if (isFirstPerson)
        {
            cameraTransform.position = targetPos;
            cameraTransform.rotation = rotation;
        }
        else
        {
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 desiredPosition = rotation * negDistance + targetPos;

            Vector3 direction = desiredPosition - targetPos;
            float maxDistance = direction.magnitude;
            if (Physics.SphereCast(targetPos, collisionRadius, direction.normalized, out RaycastHit hit, maxDistance, collisionLayers))
            {
                desiredPosition = targetPos + direction.normalized * (hit.distance - collisionRadius);
            }

            cameraTransform.position = desiredPosition;
            cameraTransform.LookAt(targetPos);
        }
    }
}

