using UnityEngine;

public class TimerSystem : MonoBehaviour
{
    private bool timerActivo = false;

    public bool TimerActivo
    {
        get { return timerActivo; }
    }

    public void IniciarTimer()
    {
        timerActivo = true;
        Debug.Log("Timer iniciado.");
    }

    public void DetenerTimer()
    {
        timerActivo = false;
        Debug.Log("Timer detenido.");
    }
}