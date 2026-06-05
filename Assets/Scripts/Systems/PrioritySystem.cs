using System.Collections.Generic;
using UnityEngine;

public class PrioritySystem : MonoBehaviour
{
    [Header("Orden de resolución")]
    public List<string> resolvedOrder = new List<string>();

    public void RegisterEvent(CrisisEvent crisisEvent)
    {
        if (crisisEvent == null) return;

        resolvedOrder.Add(crisisEvent.eventName);
    }

    public string GetOrderAsText()
    {
        if (resolvedOrder.Count == 0)
        {
            return "Sin eventos resueltos";
        }

        return string.Join(" → ", resolvedOrder);
    }
}