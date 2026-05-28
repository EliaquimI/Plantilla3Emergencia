using System.Collections.Generic;
using UnityEngine;

public class EmergencyManager : MonoBehaviour
{
    [Header("Configuración principal")]
    [SerializeField] private float tiempoInicial = 30f;
    [SerializeField] private int dañoMaximo = 100;
    [SerializeField] private int dañoPorSegundo = 5;

    [Header("Objetos que deben resolverse")]
    [SerializeField] private List<EmergencyObject> objetosEmergencia = new List<EmergencyObject>();

    [Header("Referencias del sistema")]
    [SerializeField] private TimerSystem timerSystem;
    [SerializeField] private UIManager uiManager;

    public float TiempoRestante { get; private set; }
    public int Daño { get; private set; }

    private bool crisisActiva = false;
    private bool juegoTerminado = false;

    private float acumuladorDaño = 0f;

    private void Start()
{
    Time.timeScale = 1f;

    TiempoRestante = tiempoInicial;
    Daño = 0;

    if (uiManager != null)
    {
        uiManager.ActualizarTiempo(TiempoRestante);
        uiManager.ActualizarDaño(Daño, dañoMaximo);
        uiManager.OcultarPanelResultado();
    }
}

    private void Update()
    {
        if (!crisisActiva || juegoTerminado)
        {
            return;
        }

        ProcesarTiempo();
        ProcesarDaño();
        RevisarDerrota();
        RevisarVictoria();
    }

    public void IniciarEmergencia()
    {
        if (crisisActiva || juegoTerminado)
        {
            return;
        }

        crisisActiva = true;

        if (timerSystem != null)
        {
            timerSystem.IniciarTimer();
        }

        Debug.Log("Emergencia iniciada.");
    }

    private void ProcesarTiempo()
    {
        TiempoRestante -= Time.deltaTime;

        if (TiempoRestante < 0)
        {
            TiempoRestante = 0;
        }

        if (uiManager != null)
        {
            uiManager.ActualizarTiempo(TiempoRestante);
        }
    }

    private void ProcesarDaño()
    {
        acumuladorDaño += Time.deltaTime;

        if (acumuladorDaño >= 1f)
        {
            Daño += dañoPorSegundo;
            acumuladorDaño = 0f;

            if (Daño > dañoMaximo)
            {
                Daño = dañoMaximo;
            }

            if (uiManager != null)
            {
                uiManager.ActualizarDaño(Daño, dañoMaximo);
            }
        }
    }

    public void NotificarObjetoCompletado(EmergencyObject objeto)
    {
        Debug.Log("Objeto completado: " + objeto.name);
        RevisarVictoria();
    }

    private void RevisarVictoria()
    {
        if (objetosEmergencia.Count == 0)
        {
            return;
        }

        foreach (EmergencyObject objeto in objetosEmergencia)
        {
            if (objeto == null || !objeto.EstaCompletado)
            {
                return;
            }
        }

        Ganar();
    }

    private void RevisarDerrota()
    {
        if (TiempoRestante <= 0)
        {
            Perder("Se acabó el tiempo.");
            return;
        }

        if (Daño >= dañoMaximo)
        {
            Perder("El daño llegó al máximo.");
        }
    }

    private void Ganar()
    {
        if (juegoTerminado)
        {
            return;
        }

        juegoTerminado = true;
        crisisActiva = false;

        Time.timeScale = 0f;

        if (uiManager != null)
        {
            uiManager.MostrarVictoria();
        }

        Debug.Log("Victoria: todos los objetos fueron resueltos.");
    }

    private void Perder(string razon)
    {
        if (juegoTerminado)
        {
            return;
        }

        juegoTerminado = true;
        crisisActiva = false;

        Time.timeScale = 0f;

        if (uiManager != null)
        {
            uiManager.MostrarDerrota(razon);
        }

        Debug.Log("Derrota: " + razon);
    }

    public void ReiniciarTiempoJuego()
    {
        Time.timeScale = 1f;
    }
}