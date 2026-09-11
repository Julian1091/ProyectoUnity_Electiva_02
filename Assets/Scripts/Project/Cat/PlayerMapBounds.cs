using UnityEngine;

public class PlayerMapBounds : MonoBehaviour
{
    [Header("Límites del mapa")]
    [SerializeField] private BoxCollider2D levelBounds;

    [Header("Player")]
    [SerializeField] private Collider2D playerCollider;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        LimitHorizontalMovement();
    }

    private void LimitHorizontalMovement()
    {
        if (levelBounds == null || playerCollider == null)
            return;

        Bounds mapBounds = levelBounds.bounds;
        Bounds catBounds = playerCollider.bounds;

        float halfCatWidth = catBounds.extents.x;

        float minX = mapBounds.min.x + halfCatWidth;
        float maxX = mapBounds.max.x - halfCatWidth;

        Vector2 position = rb.position;

        float clampedX = Mathf.Clamp(
            position.x,
            minX,
            maxX
        );

        // Si intentó salir del mapa
        if (!Mathf.Approximately(position.x, clampedX))
        {
            position.x = clampedX;

            // Detiene la velocidad hacia afuera
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            rb.position = position;
        }
    }
}