using UnityEngine;

public class LevelDifficulty : MonoBehaviour
{
    public ItemSpawner spawner;

    [Header("Nivel actual")]
    [Range(1, 3)]
    public int level = 1;

    private void Start()
    {
        // Si no fue asignado manualmente,
        // buscamos automáticamente el ItemSpawner de esta escena.
        if (spawner == null)
        {
            spawner = FindFirstObjectByType<ItemSpawner>();
        }

        if (spawner == null)
        {
            Debug.LogError(
                "LevelDifficulty: No encontré ningún ItemSpawner en esta escena."
            );

            return;
        }

        ApplyDifficulty();
    }

    public void ApplyDifficulty()
    {
        switch (level)
        {
            // ==========================================
            // NIVEL 1
            // ==========================================

            case 1:

                spawner.SetDifficulty(
                    1f,    // Cada 5 segundos
                    3.0f,    // Velocidad
                    0.20f    // 20% objetos malos
                );

                break;


            // ==========================================
            // NIVEL 2
            // ==========================================

            case 2:

                spawner.SetDifficulty(
                    0.5f,    // Aparecen más rápido
                    5.5f,    // Caen más rápido
                    0.35f    // 35% objetos malos
                );

                break;


            // ==========================================
            // NIVEL 3
            // ==========================================

            case 3:

                spawner.SetDifficulty(
                    0.1f,    // Mucho más frecuentes
                    7.0f,    // Mucho más rápidos
                    0.50f    // 50% objetos malos
                );

                break;
        }
    }
}