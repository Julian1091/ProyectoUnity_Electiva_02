using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        [TextArea(2, 4)]
        public string texto;

        public Sprite imagenOpcional;
    }

    [Header("Panel del tutorial")]
    public GameObject panelTutorial;
    public TMP_Text textoPaso;
    public Image imagenPaso;
    public TMP_Text textoBoton;

    [Header("Pasos del tutorial")]
    public TutorialStep[] pasos;

    [Header("Referencias")]
    public ItemSpawner itemSpawner;

    private int pasoActual = 0;
    private bool tutorialActivo = false;

    private void Start()
    {
        // Pausar el juego
        Time.timeScale = 0f;

        // Desactivar generación de objetos
        if (itemSpawner != null)
            itemSpawner.enabled = false;

        // Mostrar tutorial
        if (panelTutorial != null)
            panelTutorial.SetActive(true);

        pasoActual = 0;
        tutorialActivo = true;

        MostrarPaso(pasoActual);
    }

    private void Update()
    {
        if (!tutorialActivo)
            return;

        // CLIC CON MOUSE - para probar en Unity
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            AvanzarTutorial();
            return;
        }

        // TOUCH - para celular
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            AvanzarTutorial();
        }
    }

    private void MostrarPaso(int index)
    {
        if (pasos == null || pasos.Length == 0)
            return;

        if (index < 0 || index >= pasos.Length)
            return;

        TutorialStep paso = pasos[index];

        // Texto
        if (textoPaso != null)
            textoPaso.text = paso.texto;

        // Imagen
        if (imagenPaso != null)
        {
            if (paso.imagenOpcional != null)
            {
                imagenPaso.sprite = paso.imagenOpcional;
                imagenPaso.enabled = true;
            }
            else
            {
                imagenPaso.sprite = null;
                imagenPaso.enabled = false;
            }
        }

        // Texto del botón
        if (textoBoton != null)
        {
            if (index == pasos.Length - 1)
                textoBoton.text = "¡ENTENDIDO!";
            else
                textoBoton.text = "SIGUIENTE";
        }

        Debug.Log("Tutorial - Paso: " + (index + 1));
    }

    private void AvanzarTutorial()
    {
        pasoActual++;

        if (pasoActual >= pasos.Length)
        {
            CerrarTutorial();
        }
        else
        {
            MostrarPaso(pasoActual);
        }
    }

    private void CerrarTutorial()
    {
        tutorialActivo = false;

        if (panelTutorial != null)
            panelTutorial.SetActive(false);

        // Reanudar juego
        Time.timeScale = 1f;

        // Activar generación de objetos
        if (itemSpawner != null)
            itemSpawner.enabled = true;

        Debug.Log("Tutorial terminado");
    }
}