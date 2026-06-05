using UnityEngine;

public class CrisisEvent : MonoBehaviour
{
    public enum ConsequenceType
    {
        None,
        DamageOverTime,
        TimeDrainMultiplier,
        CriticalTimePenalty,
        CriticalDamagePenalty,
        CriticalDisableObject
    }

    [Header("Identidad del evento")]
    public string eventName = "Evento";
    public int priority = 1;

    [Header("Estado")]
    public bool startsActive = false;
    public bool isActive;
    public bool isResolved;
    public bool criticalExpired;

    [Header("Consecuencia mientras está activo")]
    public ConsequenceType activeConsequence = ConsequenceType.None;
    public int damageAmount = 5;
    public float damageInterval = 3f;
    public float timeDrainMultiplier = 1f;

    [Header("Consecuencia por tiempo crítico")]
    public bool useCriticalTime;
    public float criticalTime = 10f;
    public ConsequenceType criticalConsequence = ConsequenceType.None;
    public int criticalDamageAmount = 25;
    public float criticalTimePenalty = 15f;

    [Header("Objetos visuales")]
    public GameObject[] objectsToActivateOnEmergency;
    public GameObject[] objectsToDeactivateOnResolve;
    public GameObject[] objectsToActivateOnResolve;
    public GameObject objectToDisableOnCriticalFail;

    [Header("Audio")]
    public AudioSource audioSource;
    public bool playAudioOnActive = true;
    public bool stopAudioOnResolve = true;

    [Header("Animator opcional")]
    public Animator animator;
    public string resolveAnimationTrigger = "Open";

    private EmergencyManager emergencyManager;
    private float damageTimer;
    private float criticalTimer;

    public void Setup(EmergencyManager manager)
    {
        emergencyManager = manager;
    }

    public void DeactivateAtStart()
    {
        isResolved = false;
        criticalExpired = false;
        criticalTimer = criticalTime;
        damageTimer = 0f;

        if (startsActive)
        {
            ActivateEvent();
        }
        else
        {
            isActive = false;

            foreach (GameObject obj in objectsToActivateOnEmergency)
            {
                if (obj != null) obj.SetActive(false);
            }

            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }
    }

    public void ActivateEvent()
    {
        if (isResolved) return;

        isActive = true;
        criticalTimer = criticalTime;

        foreach (GameObject obj in objectsToActivateOnEmergency)
        {
            if (obj != null) obj.SetActive(true);
        }

        if (audioSource != null && playAudioOnActive)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void TickEvent()
    {
        if (!IsActiveAndUnresolved()) return;

        TickActiveConsequence();
        TickCriticalTime();
    }

    private void TickActiveConsequence()
    {
        switch (activeConsequence)
        {
            case ConsequenceType.DamageOverTime:
                damageTimer += Time.deltaTime;

                if (damageTimer >= damageInterval)
                {
                    damageTimer = 0f;

                    if (emergencyManager != null)
                    {
                        emergencyManager.AddDamage(damageAmount);
                    }
                }
                break;
        }
    }

    private void TickCriticalTime()
    {
        if (!useCriticalTime || criticalExpired) return;

        criticalTimer -= Time.deltaTime;

        if (criticalTimer <= 0f)
        {
            ApplyCriticalConsequence();
        }
    }

    private void ApplyCriticalConsequence()
    {
        criticalExpired = true;

        if (emergencyManager == null) return;

        switch (criticalConsequence)
        {
            case ConsequenceType.CriticalTimePenalty:
                emergencyManager.RemoveTime(criticalTimePenalty);
                break;

            case ConsequenceType.CriticalDamagePenalty:
                emergencyManager.AddDamage(criticalDamageAmount);
                break;

            case ConsequenceType.CriticalDisableObject:
                emergencyManager.AddDamage(criticalDamageAmount);

                if (objectToDisableOnCriticalFail != null)
                {
                    objectToDisableOnCriticalFail.SetActive(false);
                }
                break;
        }
    }

    public bool CanBeResolved()
    {
        return isActive && !isResolved && !criticalExpired;
    }

    public bool IsActiveAndUnresolved()
    {
    return isActive && !isResolved;
    }

   public bool ResolveEvent()
{
    if (!CanBeResolved()) return false;

    if (emergencyManager != null && !emergencyManager.CanResolveThisEvent(this))
    {
        return false;
    }

    isResolved = true;
    isActive = false;

    foreach (GameObject obj in objectsToDeactivateOnResolve)
    {
        if (obj != null) obj.SetActive(false);
    }

    foreach (GameObject obj in objectsToActivateOnResolve)
    {
        if (obj != null) obj.SetActive(true);
    }

    if (audioSource != null && stopAudioOnResolve)
    {
        audioSource.Stop();
    }

    if (animator != null && !string.IsNullOrWhiteSpace(resolveAnimationTrigger))
    {
        animator.SetTrigger(resolveAnimationTrigger);
    }

    if (emergencyManager != null)
    {
        emergencyManager.RegisterResolvedEvent(this);
    }

    return true;
}
}