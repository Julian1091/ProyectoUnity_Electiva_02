using UnityEngine;
using System.Collections;

public class FallingItem : MonoBehaviour
{
    public enum ItemType
    {
        Comida,
        Juguete,
        Atun,
        Secador,
        Basura,
        Matera,
        PowerUpIman,
        PowerUpInvulnerable
    }

    [Header("Tipo de objeto")]
    public ItemType itemType;

    [Header("Valor de comida")]
    public int foodValue = 10;

    [Header("Daño")]
    public int damage = 1;

    [Header("Caída")]
    public float fallSpeed = 18f;

    [Header("Desaparecer después de tocar el suelo")]
    public float destroyAfterSeconds = 1.5f;

    private Rigidbody2D rb;

    private bool touchedGround = false;


    // =========================================================
    // INICIO
    // =========================================================

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError(gameObject.name + " NO tiene Rigidbody2D.");
            return;
        }

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.simulated = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // 👈 AGREGA ESTO
    }


    // =========================================================
    // CAÍDA
    // =========================================================

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        // Mantener siempre la caída hacia abajo
        rb.linearVelocity = new Vector2(
            0f,
            -fallSpeed
        );
    }


    // =========================================================
    // TIPO DE OBJETO
    // =========================================================

    public bool IsPositive()
    {
        return itemType == ItemType.Comida ||
               itemType == ItemType.Juguete ||
               itemType == ItemType.Atun;
    }


    public bool IsNegative()
    {
        return itemType == ItemType.Secador ||
               itemType == ItemType.Basura ||
               itemType == ItemType.Matera;
    }


    public bool IsPowerUp()
    {
        return itemType == ItemType.PowerUpIman ||
               itemType == ItemType.PowerUpInvulnerable;
    }


    // =========================================================
    // COLISIONES
    // =========================================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ---------------------------------------------
        // TOCA EL SUELO
        // ---------------------------------------------

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log(gameObject.name + " tocó el suelo.");

            if (!touchedGround)
            {
                touchedGround = true;

                rb.linearVelocity = Vector2.zero;

                StartCoroutine(DestroyAfterTime());
            }
        }


        // ---------------------------------------------
        // TOCA AL JUGADOR
        // ---------------------------------------------

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPowerUps powerUps =
                collision.gameObject.GetComponent<PlayerPowerUps>();


            // OBJETO BUENO
            if (IsPositive())
            {
                GameManager.Instance.AddFood(foodValue);

                Destroy(gameObject);
            }


            // OBJETO MALO
            else if (IsNegative())
            {
                if (powerUps == null ||
                    !powerUps.IsInvulnerable())
                {
                    GameManager.Instance.TakeDamage(damage);
                }

                Destroy(gameObject);
            }


            // POWER UP IMÁN
            else if (itemType == ItemType.PowerUpIman)
            {
                if (powerUps != null)
                {
                    powerUps.ActivateMagnet();
                }

                Destroy(gameObject);
            }


            // POWER UP INVULNERABILIDAD
            else if (itemType == ItemType.PowerUpInvulnerable)
            {
                if (powerUps != null)
                {
                    powerUps.ActivateInvulnerability();
                }

                Destroy(gameObject);
            }
        }
    }


    // =========================================================
    // DESTRUCCIÓN
    // =========================================================

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(
            destroyAfterSeconds
        );

        Destroy(gameObject);
    }
}