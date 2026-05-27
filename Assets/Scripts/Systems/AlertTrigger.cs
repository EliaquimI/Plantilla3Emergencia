using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AlertTrigger : MonoBehaviour
{
    // Tiempo de espera antes de que explote la crisis en la oficina
    [SerializeField] private float tiempoRetraso = 3f;

    // Evento abstracto que dara la señal para encender las alarmas en el editor
    public UnityEvent alActivarAlerta; 

    // Variable interna para asegurar que la crisis solo se detone una vez
    private bool yaSeActivo = false;

    // Detecta el momento exacto en que el jugador cruza la puerta
    private void OnTriggerEnter(Collider other)
    {
        // Compara si el objeto que entro tiene la etiqueta oficial del jugador
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            yaSeActivo = true;
            // Inicia el contador de tiempo en segundo plano
            StartCoroutine(SecuenciaRetrasoCrisis());
        }
    }

    // Bloque de espera síncrono para calcular los 3 segundos
    IEnumerator SecuenciaRetrasoCrisis()
    {
        // Detiene la ejecucion durante el tiempo configurado
        yield return new WaitForSeconds(tiempoRetraso);
        
        // Si hay funciones asignadas en el inspector, las ejecuta todas de golpe
        if (alActivarAlerta != null)
        {
            alActivarAlerta.Invoke();
        }
    }
}
