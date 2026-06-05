using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Cámara")]
    public Transform cameraTransform;
    public float mouseSensitivity = 120f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;
    public bool invertMouseY = false;

    private CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity;
    private float cameraPitch;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ReadMovementInput();
        Move();
        Look();
    }

    private void ReadMovementInput()
    {
        float x = 0f;
        float z = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
        }

        moveInput = new Vector2(x, z).normalized;
    }

    private void Move()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    private void Look()
    {
        if (cameraTransform == null) return;
        if (Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(0f, mouseX, 0f);

        if (invertMouseY)
        {
            cameraPitch += mouseY;
        }
        else
        {
            cameraPitch -= mouseY;
        }

        cameraPitch = Mathf.Clamp(cameraPitch, minVerticalAngle, maxVerticalAngle);

        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }
}