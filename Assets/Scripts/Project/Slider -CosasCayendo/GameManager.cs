using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("COMIDA")]
    public Slider foodSlider;
    public float maxFood = 100f;

    [Header("TRANSFORMACIÓN DEL GATO")]
    public CatWeightController catWeightController;

    [Header("VIDAS")]
    public int maxLives = 3;
    public int currentLives;

    [Header("UI Corazones")]
    public GameObject[] hearts;

    [Header("Referencias")]              // 👈 NUEVO
    public UIManager uiManager;           // 👈 NUEVO
    public ItemSpawner itemSpawner;       // 👈 NUEVO

    private bool levelEnded = false;      // 👈 NUEVO

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentLives = maxLives;

        if (foodSlider != null)
        {
            foodSlider.minValue = 0;
            foodSlider.maxValue = maxFood;
            foodSlider.value = 0;
        }
        else
        {
            Debug.LogError("GameManager: Food Slider NO está asignado.");
        }

        UpdateHeartsUI();
    }

    public void AddFood(int amount)
    {
        if (levelEnded)
            return;

        if (foodSlider == null)
        {
            Debug.LogError(
                "No puedo sumar comida: " +
                "Food Slider no está asignado."
            );

            return;
        }

        // Sumar comida sin superar el máximo
        foodSlider.value = Mathf.Clamp(
            foodSlider.value + amount,
            0f,
            maxFood
        );

        // Convertimos el slider a porcentaje:
        // 0 = 0%
        // 0.8 = 80%
        // 1 = 100%
        float normalizedFood =
            foodSlider.value / maxFood;

        // Le avisamos al gato
        if (catWeightController != null)
        {
            catWeightController.UpdateFoodProgress(
                normalizedFood
            );
        }

        Debug.Log(
            "COMIDA: " +
            foodSlider.value +
            " / " +
            maxFood
        );

        if (foodSlider.value >= maxFood && !levelEnded)
        {
            Debug.Log(
                "¡OBJETIVO DE COMIDA COMPLETADO!"
            );

            LevelComplete();
        }
    }

    public void TakeDamage(int amount)
    {
        if (levelEnded) return;

        currentLives -= amount;

        if (currentLives < 0)
            currentLives = 0;

        // 🔊 Sonido de daño
        AudioManager.Instance?.PlayDamage();

        UpdateHeartsUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHeartsUI()
    {
        if (hearts == null || hearts.Length == 0) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentLives);
        }
    }

    // =========================================================
    // NIVEL COMPLETADO                     👈 FUNCIÓN NUEVA
    // =========================================================
    private void LevelComplete()
    {
        levelEnded = true;

        if (itemSpawner != null)
            itemSpawner.enabled = false;

        if (uiManager != null)
            uiManager.MostrarVictoria((int)foodSlider.value, currentLives);
    }

    // =========================================================
    // GAME OVER                            👈 AHORA USA UIManager
    // =========================================================
    private void GameOver()
    {
        levelEnded = true;

        if (itemSpawner != null)
            itemSpawner.enabled = false;

        if (uiManager != null)
            uiManager.MostrarGameOver();
    }
}