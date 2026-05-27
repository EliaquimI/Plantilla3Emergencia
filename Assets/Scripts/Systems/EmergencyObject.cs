using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem; // Linea obligatoria para usar el nuevo paquete de lectura de teclado

public class EmergencyObject : MonoBehaviour
{
    // Evento abstracto para conectar las consecuencias visuales o sonoras en el inspector
    public UnityEvent alMitigarEmergencia; 

    // Variable interna para saber si el jugador esta dentro de la zona verde ampliada
    private bool jugadorEnRango = false;

    // Se activa cuando el jugador entra al area del collider del objeto
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            Debug.Log("Jugador en rango. Presione E para interactuar.");
        }
    }

    // Se activa cuando el jugador camina y sale de la zona verde ampliada
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
        }
    }

    // Corre de forma constante en cada frame del juego
    private void Update()
    {
        // Verifica si el usuario cumple las dos condiciones usando el nuevo sistema: 
        // estar en rango y haber presionado la tecla E fisicamente en este frame
        if (jugadorEnRango && Keyboard.current.eKey.wasPressedThisFrame)
        {
            EjecutarMitigacion();
        }
    }

    // Realiza los cambios en el entorno del juego
    private void EjecutarMitigacion()
    {
        // Invoca todas las respuestas que le arrastraste en el editor
        if (alMitigarEmergencia != null)
        {
            alMitigarEmergencia.Invoke();
        }

        // Desactiva este script para que el jugador no pueda volver a presionar la E en este objeto
        this.enabled = false; 
    }
}
