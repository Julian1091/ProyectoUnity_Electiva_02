using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerUpPopup : MonoBehaviour
{
    public static PowerUpPopup Instance;

    [Header("Referencias UI")]
    public Text popupText;          // Si usas TextMeshPro, cambia a TMP_Text
    public CanvasGroup canvasGroup;

    [Header("Configuración")]
    public float fadeSpeed = 4f;
    public float displayDuration = 1.5f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    public void ShowMessage(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        if (popupText != null)
            popupText.text = message;

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(displayDuration);

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}