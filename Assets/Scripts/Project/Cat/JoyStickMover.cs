using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class JoyStickMover : MonoBehaviour
{
    [Header("Joystick")]
    [SerializeField] private RectTransform handle;
    [SerializeField] private float movementRange = 70f;

    // El gato seguirá utilizando solamente Horizontal
    public float Horizontal { get; private set; }

    // Lo dejamos disponible por si después quieren usar Y
    public float Vertical { get; private set; }

    private RectTransform area;
    private Canvas canvas;
    private Camera uiCamera;

    private Vector2 handleStartPosition;

    private bool mouseDragging = false;
    private int activeTouchId = -1;

    private void Awake()
    {
        area = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null &&
            canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = canvas.worldCamera;
        }
        else
        {
            uiCamera = null;
        }

        if (handle != null)
        {
            handleStartPosition = handle.anchoredPosition;
        }
    }

    private void OnEnable()
    {
        if (!EnhancedTouchSupport.enabled)
        {
            EnhancedTouchSupport.Enable();
        }
    }

    private void OnDisable()
    {
        if (EnhancedTouchSupport.enabled)
        {
            EnhancedTouchSupport.Disable();
        }
    }

    private void Update()
    {
        HandleMouse();
        HandleTouch();
    }

    // ==============================
    // MOUSE - UNITY EDITOR
    // ==============================

    private void HandleMouse()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(
                area,
                mousePosition,
                uiCamera))
            {
                mouseDragging = true;
                UpdateJoystick(mousePosition);
            }
        }

        if (mouseDragging &&
            Mouse.current.leftButton.isPressed)
        {
            UpdateJoystick(mousePosition);
        }

        if (mouseDragging &&
            Mouse.current.leftButton.wasReleasedThisFrame)
        {
            mouseDragging = false;
            ResetJoystick();
        }
    }

    // ==============================
    // TOUCH - ANDROID
    // ==============================

    private void HandleTouch()
    {
        foreach (Touch touch in Touch.activeTouches)
        {
            if (activeTouchId == -1 &&
                touch.phase ==
                UnityEngine.InputSystem.TouchPhase.Began)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(
                    area,
                    touch.screenPosition,
                    uiCamera))
                {
                    activeTouchId = touch.touchId;
                    UpdateJoystick(touch.screenPosition);
                }
            }

            if (touch.touchId == activeTouchId)
            {
                if (touch.phase ==
                        UnityEngine.InputSystem.TouchPhase.Moved ||
                    touch.phase ==
                        UnityEngine.InputSystem.TouchPhase.Stationary)
                {
                    UpdateJoystick(touch.screenPosition);
                }

                if (touch.phase ==
                        UnityEngine.InputSystem.TouchPhase.Ended ||
                    touch.phase ==
                        UnityEngine.InputSystem.TouchPhase.Canceled)
                {
                    activeTouchId = -1;
                    ResetJoystick();
                }
            }
        }
    }

    // ==============================
    // MOVIMIENTO 360° DEL JOYSTICK
    // ==============================

    private void UpdateJoystick(Vector2 screenPosition)
    {
        if (handle == null)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            area,
            screenPosition,
            uiCamera,
            out Vector2 localPoint))
        {
            // Convertimos la posición del dedo respecto
            // al centro del área del joystick
            Vector2 input = new Vector2(
                localPoint.x / (area.rect.width / 2f),
                localPoint.y / (area.rect.height / 2f)
            );

            // Evita que salga del círculo
            input = Vector2.ClampMagnitude(input, 1f);

            // Zona muerta pequeña
            if (input.magnitude < 0.08f)
            {
                input = Vector2.zero;
            }

            Horizontal = input.x;
            Vertical = input.y;

            // Ahora el círculo se mueve tanto en X como en Y
            handle.anchoredPosition =
                handleStartPosition +
                input * movementRange;
        }
    }

    private void ResetJoystick()
    {
        Horizontal = 0f;
        Vertical = 0f;

        if (handle != null)
        {
            handle.anchoredPosition =
                handleStartPosition;
        }
    }
}