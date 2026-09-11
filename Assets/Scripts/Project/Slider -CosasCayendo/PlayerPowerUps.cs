using UnityEngine;

public class PlayerPowerUps : MonoBehaviour


{
    [Header("Duración del imán")]
    public float magnetDuration = 8f;


    [Header("Configuración del imán")]
    public float magnetRadius = 5f;
    public float magnetForce = 10f;

    [Header("Duración de invulnerabilidad")]
    public float invulnerabilityDuration = 5f;

    private bool magnetActive = false;
    private bool invulnerable = false;

    // =========================================================
    // IMÁN
    // =========================================================

    public void ActivateMagnet()
    {
        StopCoroutine(nameof(MagnetCoroutine));
        StartCoroutine(MagnetCoroutine());
    }

    private void Update()
    {
        if (magnetActive)
        {
            AttractFood();
        }
    }

    private void AttractFood()
    {
        Collider2D[] objects =
            Physics2D.OverlapCircleAll(
                transform.position,
                magnetRadius
            );

        foreach (Collider2D obj in objects)
        {
            FallingItem item =
                obj.GetComponent<FallingItem>();

            if (item != null && item.IsPositive())
            {
                Vector2 direction =
                    transform.position - obj.transform.position;

                Rigidbody2D rb =
                    obj.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    rb.AddForce(
                        direction.normalized * magnetForce
                    );
                }
            }
        }
    }
    private System.Collections.IEnumerator MagnetCoroutine()
    {
        magnetActive = true;
        Debug.Log("¡Imán activado!");
        PowerUpPopup.Instance?.ShowMessage("🧲 ¡Imán activado!");

        yield return new WaitForSeconds(magnetDuration);
        magnetActive = false;
        Debug.Log("¡Imán terminado!");
    }

    public void ActivateInvulnerability()
    {
        StopCoroutine(nameof(InvulnerabilityCoroutine));
        StartCoroutine(InvulnerabilityCoroutine());
    }

    private System.Collections.IEnumerator InvulnerabilityCoroutine()
    {
        invulnerable = true;
        Debug.Log("¡Invulnerabilidad activada!");
        PowerUpPopup.Instance?.ShowMessage("✨ ¡Invulnerabilidad activada!");

        yield return new WaitForSeconds(invulnerabilityDuration);
        invulnerable = false;
        Debug.Log("¡Invulnerabilidad terminada!");
    }

    // =========================================================
    // INVULNERABILIDAD
    // =========================================================

    


    // =========================================================
    // CONSULTAS
    // =========================================================

    public bool IsMagnetActive()
    {
        return magnetActive;
    }

    public bool IsInvulnerable()
    {
        return invulnerable;
    }
}