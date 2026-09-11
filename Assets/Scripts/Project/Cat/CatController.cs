using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class CatController : MonoBehaviour
{
    // =========================================================
    // MOVIMIENTO DEL GATO
    // =========================================================

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Joystick")]
    [SerializeField] private JoyStickMover joystick;

    // =========================================================
    // ANIMACIONES
    // =========================================================

    [Header("Animacion")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // =========================================================
    // GROUND CHECK
    // =========================================================

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    // =========================================================
    // INTERFAZ / HUD
    // =========================================================

    [Header("Conexion con la Interfaz (UI)")]
    public UIManager uiManager;

    [Header("HUD Superior en Pantalla")]
    public Slider barraComida;
    public TextMeshProUGUI txtContadorVidas;

    [Header("Estadisticas del Gato")]
    public int vidas = 3;
    public int comidaRecolectada = 0;
    public int metaComida = 10;


    // =========================================================
    // UNITY
    // =========================================================

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Configurar barra de comida
        if (barraComida != null)
        {
            barraComida.minValue = 0;
            barraComida.maxValue = metaComida;
            barraComida.value = comidaRecolectada;
        }

        ActualizarHUD();
    }

    private void Update()
    {
        CheckGround();
        UpdateAnimations();
        FlipCat();

        // -----------------------------------------------------
        // TECLAS DE PRUEBA EN PC
        // -----------------------------------------------------

        // C = simular recoger comida
        if (Keyboard.current != null &&
            Keyboard.current.cKey.wasPressedThisFrame)
        {
            SumarComida(1);
        }

        // X = simular recibir daño
        if (Keyboard.current != null &&
            Keyboard.current.xKey.wasPressedThisFrame)
        {
            RecibirDano(1);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }


    // =========================================================
    // MOVIMIENTO
    // =========================================================

    private void Move()
    {
        if (joystick == null || rb == null)
            return;

        float horizontal = joystick.Horizontal;

        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y
        );
    }


    // =========================================================
    // SALTO
    // =========================================================

    public void Jump()
    {
        if (!isGrounded)
            return;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );

        AudioManager.Instance?.PlayJump();
    }


    // =========================================================
    // GROUND CHECK
    // =========================================================

    private void CheckGround()
    {
        if (rb.linearVelocity.y > 0.1f)
        {
            isGrounded = false;
            return;
        }

        RaycastHit2D hit = Physics2D.BoxCast(
            groundCheck.position,
            new Vector2(0.35f, 0.08f),
            0f,
            Vector2.down,
            0.15f,
            groundLayer
        );

        isGrounded = hit.collider != null;
    }


    // =========================================================
    // ANIMACIONES
    // =========================================================

    private void UpdateAnimations()
    {
        if (animator == null || rb == null)
            return;

        float horizontal = joystick != null
            ? joystick.Horizontal
            : 0f;

        animator.SetFloat(
            "Speed",
            Mathf.Abs(horizontal)
        );

        animator.SetBool(
            "IsGrounded",
            isGrounded
        );

        animator.SetFloat(
            "YVelocity",
            rb.linearVelocity.y
        );
    }


    // =========================================================
    // DIRECCION DEL GATO
    // =========================================================

    private void FlipCat()
    {
        if (spriteRenderer == null || joystick == null)
            return;

        if (joystick.Horizontal > 0.05f)
        {
            // Derecha
            spriteRenderer.flipX = true;
        }
        else if (joystick.Horizontal < -0.05f)
        {
            // Izquierda
            spriteRenderer.flipX = false;
        }
    }


    // =========================================================
    // HUD
    // =========================================================

    private void ActualizarHUD()
    {
        if (barraComida != null)
        {
            barraComida.value = comidaRecolectada;
        }

        if (txtContadorVidas != null)
        {
            txtContadorVidas.text = "Vidas: " + vidas;
        }
    }


    // =========================================================
    // COMIDA
    // =========================================================

    public void SumarComida(int puntos)
    {
        comidaRecolectada += puntos;

        ActualizarHUD();

        if (comidaRecolectada >= metaComida)
        {
            if (uiManager != null)
            {
                uiManager.MostrarVictoria(
                    comidaRecolectada,
                    vidas
                );
            }
        }
    }


    // =========================================================
    // DAÑO / VIDAS
    // =========================================================

    public void RecibirDano(int cantidad)
    {
        vidas -= cantidad;

        if (vidas < 0)
            vidas = 0;

        ActualizarHUD();

        if (vidas <= 0)
        {
            if (uiManager != null)
            {
                uiManager.MostrarGameOver();
            }
        }
    }


    // =========================================================
    // GIZMOS
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.DrawWireCube(
            groundCheck.position + Vector3.down * 0.075f,
            new Vector3(0.35f, 0.15f, 0f)
        );
    }


}