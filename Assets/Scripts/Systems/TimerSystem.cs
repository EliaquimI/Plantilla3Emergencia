using UnityEngine;

public class TimerSystem : MonoBehaviour
{
    private EmergencyManager emergencyManager;

    public void Setup(EmergencyManager manager)
    {
        emergencyManager = manager;
    }

    public void Tick()
    {
        if (emergencyManager == null) return;

        float multiplier = emergencyManager.GetGlobalTimeMultiplier();
        emergencyManager.tiempoRestante -= Time.deltaTime * multiplier;
        emergencyManager.tiempoRestante = Mathf.Max(0f, emergencyManager.tiempoRestante);
    }
}