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

    private float x = 0f;
    private float y = 0f;
    private Vector2 lookInput;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Camera.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Camera.Look.canceled += _ => lookInput = Vector2.zero;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void LateUpdate()
    {
        // Zoom with mouse scroll wheel using Input System directly
        if (Mouse.current != null)
        {
            float scrollValue = Mouse.current.scroll.ReadValue().y;
            distance -= scrollValue * 0.5f; // Adjust zoom speed
            distance = Mathf.Clamp(distance, 2f, 20f); // Min and max zoom distance
        }

        // Only rotate camera if RMB is held
        if (Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            x += lookInput.x * xSpeed * Time.deltaTime;
            y -= lookInput.y * ySpeed * Time.deltaTime;
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;
        cameraTransform.position = position;

        cameraTransform.LookAt(target.position + Vector3.up * 1.5f);
    }

}
