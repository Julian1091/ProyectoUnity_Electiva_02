using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Paneles de la Interfaz")]
    public GameObject panelPausa;
    public GameObject panelGameOver;
    public GameObject panelVictoria;

    [Header("Textos del Panel de Victoria")]
    public TextMeshProUGUI txtTiempoVictoria;
    public TextMeshProUGUI txtPuntajeVictoria;
    public TextMeshProUGUI txtVidasVictoria;

    [Header("Siguiente Escena")]                                   // 👈 NUEVO
    [Tooltip("Nombre EXACTO de la escena siguiente. Vacío = último nivel.")]
    public string nextSceneName;                                    // 👈 NUEVO

    private float tiempoTranscurrido = 0f;
    private bool nivelFinalizado = false;

    void Start()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);

        Time.timeScale = 1f;
        tiempoTranscurrido = 0f;
        nivelFinalizado = false;
    }

    void Update()
    {
        if (!nivelFinalizado && Time.timeScale > 0f)
        {
            tiempoTranscurrido += Time.deltaTime;
        }
    }

    public void MostrarVictoria(int puntajeFinal, int vidasRestantes)
    {
        nivelFinalizado = true;
        Time.timeScale = 0f;

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);

            int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60);
            int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60);

            if (txtTiempoVictoria != null)
                txtTiempoVictoria.text = "Tiempo: " + minutos.ToString("00") + ":" + segundos.ToString("00");

            if (txtPuntajeVictoria != null)
                txtPuntajeVictoria.text = "Comida: " + puntajeFinal + " pts";

            if (txtVidasVictoria != null)
                txtVidasVictoria.text = "Vidas restantes: " + vidasRestantes;
        }
    }

    public void PausarJuego()
    {
        if (panelPausa != null) panelPausa.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReanudarJuego()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MostrarGameOver()
    {
        nivelFinalizado = true;
        if (panelGameOver != null) panelGameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // =========================================================
    // SIGUIENTE NIVEL                     👈 FUNCIÓN NUEVA
    // =========================================================
    public void IrASiguienteNivel()
    {
        Time.timeScale = 1f;

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("No hay siguiente escena configurada (¿último nivel?).");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}