using UnityEngine;

public class StateController : MonoBehaviour
{
    [Header("Luces")]
    public GameObject lucesBlancas;
    public GameObject lucesRojas;

    [Header("Objetos al iniciar emergencia")]
    public GameObject[] activateOnEmergency;
    public GameObject[] deactivateOnEmergency;

    [Header("Objetos al resolver emergencia")]
    public GameObject[] activateOnResolved;
    public GameObject[] deactivateOnResolved;

    public void SetEmergencyVisualState()
    {
        if (lucesBlancas != null) lucesBlancas.SetActive(false);
        if (lucesRojas != null) lucesRojas.SetActive(true);

        foreach (GameObject obj in activateOnEmergency)
        {
            if (obj != null) obj.SetActive(true);
        }

        foreach (GameObject obj in deactivateOnEmergency)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    public void SetResolvedVisualState()
    {
        if (lucesBlancas != null) lucesBlancas.SetActive(true);
        if (lucesRojas != null) lucesRojas.SetActive(false);

        foreach (GameObject obj in activateOnResolved)
        {
            if (obj != null) obj.SetActive(true);
        }

        foreach (GameObject obj in deactivateOnResolved)
        {
            if (obj != null) obj.SetActive(false);
        }
    }
}