using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EmergencyObject : MonoBehaviour
{
    [Header("Evento visual o sonoro")]
    public UnityEvent alMitigarEmergencia;

    [Header("Manager de emergencia")]
    [SerializeField] private EmergencyManager emergencyManager;

    public bool EstaCompletado { get; private set; }

    private bool jugadorEnRango = false;

    private void Start()
    {
        EstaCompletado = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !EstaCompletado)
        {
            jugadorEnRango = true;
            Debug.Log("Jugador en rango de " + gameObject.name + ". Presione E para interactuar.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (jugadorEnRango && !EstaCompletado && Keyboard.current.eKey.wasPressedThisFrame)
        {
            EjecutarMitigacion();
        }
    }

    private void EjecutarMitigacion()
    {
        EstaCompletado = true;

        if (alMitigarEmergencia != null)
        {
            alMitigarEmergencia.Invoke();
        }

        if (emergencyManager != null)
        {
            emergencyManager.NotificarObjetoCompletado(this);
        }

        jugadorEnRango = false;

        Debug.Log(gameObject.name + " fue completado.");
    }
}