using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interacción")]
    public float interactionDistance = 8f;
    public float detectionRadius = 0.45f;
    public Transform cameraTransform;
    public LayerMask interactionMask = ~0;

    [Header("UI")]
    public UIManager uiManager;
    public string mensajeInteractuar = "Presiona E para interactuar";

    private CrisisEvent currentEvent;

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        DetectInteractable();

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void DetectInteractable()
    {
        currentEvent = null;

        if (cameraTransform == null) return;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactionDistance, Color.green);

        RaycastHit[] hits = Physics.SphereCastAll(
            ray,
            detectionRadius,
            interactionDistance,
            interactionMask,
            QueryTriggerInteraction.Collide
        );

        float closestDistance = Mathf.Infinity;
        CrisisEvent closestEvent = null;

        foreach (RaycastHit hit in hits)
        {
            CrisisEvent crisisEvent = hit.collider.GetComponentInParent<CrisisEvent>();

            if (crisisEvent == null)
            {
                crisisEvent = hit.collider.GetComponent<CrisisEvent>();
            }

            if (crisisEvent == null) continue;

            if (!crisisEvent.CanBeResolved()) continue;

            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                closestEvent = crisisEvent;
            }
        }

        if (closestEvent != null)
        {
            currentEvent = closestEvent;

            if (uiManager != null)
            {
                uiManager.ShowMessage(mensajeInteractuar + ": " + currentEvent.eventName);
            }

            return;
        }

        if (uiManager != null)
        {
            uiManager.HideMessage();
        }
    }

    private void Interact()
    {
        if (currentEvent == null)
        {
            if (uiManager != null)
            {
                uiManager.ShowTemporaryMessage("No hay objeto interactuable enfrente.", 2f);
            }

            return;
        }

        currentEvent.ResolveEvent();
    }
}