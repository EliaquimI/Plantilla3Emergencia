using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI del tiempo")]
    [SerializeField] private TMP_Text textoTiempo;

    [Header("UI del daño")]
    [SerializeField] private Slider barraDaño;

    [Header("Panel de resultado")]
    [SerializeField] private GameObject panelResultado;
    [SerializeField] private TMP_Text textoTituloResultado;
    [SerializeField] private TMP_Text textoDescripcionResultado;

    public void ActualizarTiempo(float tiempoRestante)
    {
        if (textoTiempo != null)
        {
            int segundos = Mathf.CeilToInt(tiempoRestante);
            textoTiempo.text = "Tiempo: " + segundos.ToString();
        }
    }

    public void ActualizarDaño(int dañoActual, int dañoMaximo)
    {
        if (barraDaño != null)
        {
            barraDaño.maxValue = dañoMaximo;
            barraDaño.value = dañoActual;
        }
    }

    public void OcultarPanelResultado()
    {
        if (panelResultado != null)
        {
            panelResultado.SetActive(false);
        }
    }

    public void MostrarVictoria()
    {
        if (panelResultado != null)
        {
            panelResultado.SetActive(true);
        }

        if (textoTituloResultado != null)
        {
            textoTituloResultado.text = "ÉXITO";
        }

        if (textoDescripcionResultado != null)
        {
            textoDescripcionResultado.text = "Controlaste la emergencia a tiempo.";
        }
    }

    public void MostrarDerrota(string razon)
    {
        if (panelResultado != null)
        {
            panelResultado.SetActive(true);
        }

        if (textoTituloResultado != null)
        {
            textoTituloResultado.text = "DERROTA";
        }

        if (textoDescripcionResultado != null)
        {
            textoDescripcionResultado.text = razon;
        }
    }
}