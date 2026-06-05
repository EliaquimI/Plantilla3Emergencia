using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Textos")]
    public TMP_Text timerText;
    public TMP_Text damageText;
    public TMP_Text eventsText;
    public TMP_Text objectiveText;
    public TMP_Text messageText;

    [Header("Barra de daño")]
    public Slider damageBar;

    private float messageTimer;
    private bool usingTemporaryMessage;

    private void Update()
    {
        if (!usingTemporaryMessage) return;

        messageTimer -= Time.deltaTime;

        if (messageTimer <= 0f)
        {
            HideMessage();
        }
    }

    public void UpdateUI(float tiempoRestante, int daño, int eventosResueltos, int totalEventos)
    {
        if (timerText != null)
        {
            timerText.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante);
        }

        if (damageText != null)
        {
            damageText.text = "Daño: " + daño + "%";
        }

        if (eventsText != null)
        {
            eventsText.text = "Eventos resueltos: " + eventosResueltos + "/" + totalEventos;
        }

        if (damageBar != null)
        {
            damageBar.value = daño;
        }
    }

    public void SetObjective(string objective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = "Objetivo: " + objective;
        }
    }

   public void ShowMessage(string message)
{
    if (messageText == null) return;

    // Si hay un mensaje temporal activo, no lo reemplaces.
    // Esto evita que "Presiona E" borre "Orden incorrecto".
    if (usingTemporaryMessage) return;

    messageText.gameObject.SetActive(true);
    messageText.text = message;
}

    public void ShowTemporaryMessage(string message, float duration)
    {
        if (messageText == null) return;

        usingTemporaryMessage = true;
        messageTimer = duration;
        messageText.gameObject.SetActive(true);
        messageText.text = message;
    }

  public void HideMessage()
{
    // Si hay un mensaje temporal activo, no lo borres.
    if (usingTemporaryMessage) return;

    if (messageText != null)
    {
        messageText.text = "";
        messageText.gameObject.SetActive(false);
    }
}
}