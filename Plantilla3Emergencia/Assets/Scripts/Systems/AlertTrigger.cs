using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AlertTrigger : MonoBehaviour
{
    [Header("Configuración del detonante")]
    [SerializeField] private float tiempoRetraso = 3f;

    [Header("Eventos al activar la alerta")]
    public UnityEvent alActivarAlerta;

    private bool yaSeActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            yaSeActivo = true;
            StartCoroutine(SecuenciaRetrasoCrisis());
        }
    }

    private IEnumerator SecuenciaRetrasoCrisis()
    {
        yield return new WaitForSeconds(tiempoRetraso);

        if (alActivarAlerta != null)
        {
            alActivarAlerta.Invoke();
        }

        Debug.Log("La crisis fue detonada.");
    }
}