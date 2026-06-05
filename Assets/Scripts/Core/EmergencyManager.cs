using System.Collections.Generic;
using UnityEngine;

public class EmergencyManager : MonoBehaviour
{
    [Header("Estado general")]
    public bool emergencyStarted;
    public bool gameEnded;

    [Header("Variables principales")]
    public float tiempoRestante = 120f;
    public int daño = 0;

    [Header("Límites")]
    public int dañoMaximo = 100;
    public int totalEventosParaGanar = 3;

    [Header("Validación de prioridad")]
    public bool validarOrdenCorrecto = true;
    public int eventoActualIndex = 0;

    [Header("Sistemas")]
    public TimerSystem timerSystem;
    public PrioritySystem prioritySystem;
    public UIManager uiManager;
    public ResultUI resultUI;
    public StateController stateController;

    [Header("Eventos del nivel en orden correcto")]
    public List<CrisisEvent> crisisEvents = new List<CrisisEvent>();

    private int eventosResueltos;

    private void Start()
    {
        emergencyStarted = false;
        gameEnded = false;
        eventosResueltos = 0;
        eventoActualIndex = 0;

        if (timerSystem != null)
        {
            timerSystem.Setup(this);
        }

        foreach (CrisisEvent crisisEvent in crisisEvents)
        {
            if (crisisEvent != null)
            {
                crisisEvent.Setup(this);
                crisisEvent.DeactivateAtStart();
            }
        }

        if (uiManager != null)
        {
            uiManager.UpdateUI(tiempoRestante, daño, eventosResueltos, totalEventosParaGanar);
            uiManager.HideMessage();
        }

        if (resultUI != null)
        {
            resultUI.HideResult();
        }
    }

    private void Update()
    {
        if (!emergencyStarted || gameEnded) return;

        if (timerSystem != null)
        {
            timerSystem.Tick();
        }

        foreach (CrisisEvent crisisEvent in crisisEvents)
        {
            if (crisisEvent != null)
            {
                crisisEvent.TickEvent();
            }
        }

        if (uiManager != null)
        {
            uiManager.UpdateUI(tiempoRestante, daño, eventosResueltos, totalEventosParaGanar);
        }

        CheckEndConditions();
    }

    public void StartEmergency()
    {
        if (emergencyStarted || gameEnded) return;

        emergencyStarted = true;

        if (stateController != null)
        {
            stateController.SetEmergencyVisualState();
        }

        foreach (CrisisEvent crisisEvent in crisisEvents)
        {
            if (crisisEvent != null)
            {
                crisisEvent.ActivateEvent();
            }
        }

   if (uiManager != null)
{
    uiManager.ShowTemporaryMessage("Emergencia iniciada. Repara primero: " + GetCurrentObjectiveName(), 3f);
    uiManager.SetObjective(GetCurrentObjectiveName());
}
    }
public bool CanResolveThisEvent(CrisisEvent selectedEvent)
{
    if (selectedEvent == null) return false;
    if (gameEnded) return false;
    if (!emergencyStarted)
    {
        if (uiManager != null)
        {
            uiManager.ShowTemporaryMessage("La emergencia todavía no ha iniciado.", 2f);
        }

        return false;
    }

    if (!validarOrdenCorrecto)
    {
        return true;
    }

    if (eventoActualIndex >= crisisEvents.Count)
    {
        return false;
    }

    CrisisEvent expectedEvent = crisisEvents[eventoActualIndex];

    if (selectedEvent == expectedEvent)
    {
        return true;
    }

    if (uiManager != null)
    {
        uiManager.ShowTemporaryMessage(
            "Orden incorrecto. Primero debes reparar: " + expectedEvent.eventName,
            3f
        );
    }

    return false;
}

  public void RegisterResolvedEvent(CrisisEvent resolvedEvent)
{
    eventosResueltos++;

    if (prioritySystem != null)
    {
        prioritySystem.RegisterEvent(resolvedEvent);
    }

    if (validarOrdenCorrecto)
    {
        eventoActualIndex++;
    }

    if (eventosResueltos >= totalEventosParaGanar)
    {
        WinGame();
        return;
    }

    if (uiManager != null)
    {
        uiManager.ShowTemporaryMessage(
            "Evento resuelto: " + resolvedEvent.eventName + ". Siguiente objetivo: " + GetCurrentObjectiveName(),
            4f
        );

        uiManager.SetObjective(GetCurrentObjectiveName());
    }
}

    public string GetCurrentObjectiveName()
    {
        if (crisisEvents == null || crisisEvents.Count == 0)
        {
            return "Sin objetivo";
        }

        if (eventoActualIndex >= crisisEvents.Count)
        {
            return "Todos los eventos resueltos";
        }

        if (crisisEvents[eventoActualIndex] == null)
        {
            return "Objetivo no asignado";
        }

        return crisisEvents[eventoActualIndex].eventName;
    }

    public void AddDamage(int amount)
    {
        if (gameEnded) return;

        daño += amount;
        daño = Mathf.Clamp(daño, 0, dañoMaximo);
    }

    public void RemoveTime(float amount)
    {
        if (gameEnded) return;

        tiempoRestante -= amount;
        tiempoRestante = Mathf.Max(0f, tiempoRestante);
    }

    public float GetGlobalTimeMultiplier()
    {
        float multiplier = 1f;

        foreach (CrisisEvent crisisEvent in crisisEvents)
        {
            if (crisisEvent != null && crisisEvent.IsActiveAndUnresolved())
            {
                multiplier = Mathf.Max(multiplier, crisisEvent.timeDrainMultiplier);
            }
        }

        return multiplier;
    }

    private void CheckEndConditions()
    {
        if (tiempoRestante <= 0f)
        {
            LoseGame("Tiempo agotado");
            return;
        }

        if (daño >= dañoMaximo)
        {
            LoseGame("Daño crítico alcanzado");
            return;
        }
    }

    private void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (stateController != null)
        {
            stateController.SetResolvedVisualState();
        }

        if (resultUI != null)
        {
            resultUI.ShowResult(
                true,
                tiempoRestante,
                daño,
                eventosResueltos,
                totalEventosParaGanar,
                "Emergencia controlada correctamente"
            );
        }

        UnlockCursor();
    }

    public void LoseGame(string reason)
    {
        if (gameEnded) return;

        gameEnded = true;

        if (resultUI != null)
        {
            resultUI.ShowResult(
                false,
                tiempoRestante,
                daño,
                eventosResueltos,
                totalEventosParaGanar,
                reason
            );
        }

        UnlockCursor();
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}