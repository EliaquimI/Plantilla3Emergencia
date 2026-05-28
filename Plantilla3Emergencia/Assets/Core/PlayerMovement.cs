using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float gravedad = -9.81f;

    [Header("Cámara")]
    [SerializeField] private Transform camaraJugador;
    [SerializeField] private float sensibilidadMouse = 120f;

    [Header("Invertir controles")]
    [SerializeField] private bool invertirMovimientoX = false;
    [SerializeField] private bool invertirMovimientoY = false;
    [SerializeField] private bool invertirMouseX = false;
    [SerializeField] private bool invertirMouseY = false;

    private CharacterController characterController;
    private float velocidadVertical;
    private float rotacionVertical;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("Falta CharacterController en el Player_Controller.");
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MoverJugador();
        MoverCamara();
    }

    private void MoverJugador()
    {
        if (Keyboard.current == null || characterController == null)
        {
            return;
        }

        float movimientoX = 0f;
        float movimientoY = 0f;

        if (Keyboard.current.wKey.isPressed)
        {
            movimientoY += 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            movimientoY -= 1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            movimientoX += 1f;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            movimientoX -= 1f;
        }

        if (invertirMovimientoX)
        {
            movimientoX *= -1f;
        }

        if (invertirMovimientoY)
        {
            movimientoY *= -1f;
        }

        Vector3 direccion = transform.right * movimientoX + transform.forward * movimientoY;

        if (direccion.magnitude > 1f)
        {
            direccion.Normalize();
        }

        if (characterController.isGrounded && velocidadVertical < 0)
        {
            velocidadVertical = -2f;
        }

        velocidadVertical += gravedad * Time.deltaTime;

        Vector3 movimientoFinal = direccion * velocidad;
        movimientoFinal.y = velocidadVertical;

        characterController.Move(movimientoFinal * Time.deltaTime);
    }

    private void MoverCamara()
    {
        if (Mouse.current == null || camaraJugador == null)
        {
            return;
        }

        Vector2 movimientoMouse = Mouse.current.delta.ReadValue();

        float mouseX = movimientoMouse.x * sensibilidadMouse * Time.deltaTime;
        float mouseY = movimientoMouse.y * sensibilidadMouse * Time.deltaTime;

        if (invertirMouseX)
        {
            mouseX *= -1f;
        }

        if (invertirMouseY)
        {
            mouseY *= -1f;
        }

        transform.Rotate(Vector3.up * mouseX);

        rotacionVertical -= mouseY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -80f, 80f);

        camaraJugador.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
    }
}