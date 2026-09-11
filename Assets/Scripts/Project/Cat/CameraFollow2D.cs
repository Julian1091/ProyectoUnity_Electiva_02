using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform target;

    [Header("Movimiento")]
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private Vector2 offset;

    [Header("Limites del nivel")]
    [SerializeField] private BoxCollider2D levelBounds;

    private Vector3 velocity;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );

        if (levelBounds != null)
        {
            Bounds bounds = levelBounds.bounds;

            float cameraHalfHeight = cam.orthographicSize;
            float cameraHalfWidth =
                cameraHalfHeight * cam.aspect;

            targetPosition.x = Mathf.Clamp(
                targetPosition.x,
                bounds.min.x + cameraHalfWidth,
                bounds.max.x - cameraHalfWidth
            );

            targetPosition.y = Mathf.Clamp(
                targetPosition.y,
                bounds.min.y + cameraHalfHeight,
                bounds.max.y - cameraHalfHeight
            );
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}