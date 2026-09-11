using UnityEngine;
using System.Collections;

public class CatWeightController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Transformación")]
    [Range(0f, 1f)]
    [SerializeField] private float fatThreshold = 0.8f;

    [Header("Efecto visual")]
    [SerializeField] private Color flashColor = Color.yellow;
    [SerializeField] private int flashCount = 4;
    [SerializeField] private float flashDuration = 0.08f;

    private bool isFat = false;
    private bool transforming = false;

    public void UpdateFoodProgress(float normalizedFood)
    {
        if (isFat || transforming)
            return;

        if (normalizedFood >= fatThreshold)
        {
            StartCoroutine(TransformToFat());
        }
    }

    private IEnumerator TransformToFat()
    {
        transforming = true;
        isFat = true;

        // 🔊 Sonido especial
        AudioManager.Instance?.PlayTransform();

        Color originalColor = Color.white;

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        // ✨ Parpadeo
        for (int i = 0; i < flashCount; i++)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = flashColor;

            yield return new WaitForSeconds(flashDuration);

            if (spriteRenderer != null)
                spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(flashDuration);
        }

        // 🐱 → 🐱 GORDITO
        if (animator != null)
        {
            animator.SetBool("IsFat", true);
        }

        // Pequeño flash final
        if (spriteRenderer != null)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }

        transforming = false;
    }

    public bool IsFat()
    {
        return isFat;
    }
}