using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject resultPanel;
    public GameObject hudPanel;

    [Header("Botones")]
    public GameObject restartButton;
    public GameObject nextLevelButton;

    [Header("Textos")]
    public TMP_Text titleText;
    public TMP_Text finalTimeText;
    public TMP_Text finalDamageText;
    public TMP_Text finalEventsText;
    public TMP_Text messageText;

    public void HideResult()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        if (hudPanel != null)
        {
            hudPanel.SetActive(true);
        }
    }

    public void ShowResult(
        bool victory,
        float tiempoRestante,
        int daño,
        int eventosResueltos,
        int totalEventos,
        string message
    )
    {
        if (hudPanel != null)
        {
            hudPanel.SetActive(false);
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        if (restartButton != null)
        {
            restartButton.SetActive(!victory);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(victory);
        }

        if (titleText != null)
        {
            titleText.text = victory ? "VICTORIA" : "DERROTA";
        }

        if (finalTimeText != null)
        {
            finalTimeText.text = "Tiempo restante: " + Mathf.CeilToInt(tiempoRestante);
        }

        if (finalDamageText != null)
        {
            finalDamageText.text = "Daño final: " + daño + "%";
        }

        if (finalEventsText != null)
        {
            finalEventsText.text = "Eventos resueltos: " + eventosResueltos + "/" + totalEventos;
        }

        if (messageText != null)
        {
            messageText.text = message;
        }
    }
}