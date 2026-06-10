using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Button botonReinicio;
    [SerializeField] private TMP_Text textoBotonReinicio;

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

        if (textoBotonReinicio != null)
        {
            textoBotonReinicio.text = "Volver a jugar";
        }

        MostrarCursor();
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

        if (textoBotonReinicio != null)
        {
            textoBotonReinicio.text = "Reiniciar";
        }

        MostrarCursor();
    }

    private void MostrarCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReiniciarEscena()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
