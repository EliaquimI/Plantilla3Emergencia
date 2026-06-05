using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    [Header("Manager")]
    public EmergencyManager emergencyManager;

    [Header("Configuración")]
    public bool triggerOnlyOnce = true;
    public float startDelay = 2f;

    private bool alreadyTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyTriggered && triggerOnlyOnce) return;

        if (other.CompareTag("Player"))
        {
            alreadyTriggered = true;
            Invoke(nameof(StartEmergency), startDelay);
        }
    }

    private void StartEmergency()
    {
        if (emergencyManager != null)
        {
            emergencyManager.StartEmergency();
        }
    }
}