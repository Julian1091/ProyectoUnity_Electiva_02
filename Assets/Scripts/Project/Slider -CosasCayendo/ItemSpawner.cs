using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Punto de aparición")]
    public Transform spawnPoint;

    [Header("Ancho de la zona de aparición")]
    public float spawnWidth = 6f;

    [Header("Objetos buenos")]
    public GameObject[] positiveItems;

    [Header("Objetos malos")]
    public GameObject[] negativeItems;

    [Header("Power Ups")]
    public GameObject[] powerUps;

    [Header("Configuración")]
    public float spawnInterval = 0.5f;
    public float fallSpeed = 5f;

    [Range(0f, 1f)]
    public float negativeChance = 0.30f;

    [Range(0f, 1f)]
    public float powerUpChance = 0.10f;

    private float timer = 0f;


    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnItem();

            timer = 0f;
        }
    }


    private void SpawnItem()
    {
        GameObject prefab = GetRandomItem();

        if (prefab == null)
        {
            Debug.LogWarning(
                "No hay objetos configurados en el Spawner."
            );

            return;
        }


        // =====================================================
        // X ALEATORIO
        // =====================================================

        float randomX = Random.Range(
            -spawnWidth,
            spawnWidth
        );


        Vector3 spawnPosition = new Vector3(
            spawnPoint.position.x + randomX,
            spawnPoint.position.y,
            spawnPoint.position.z
        );


        // =====================================================
        // CREAR OBJETO
        // =====================================================

        GameObject newItem = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );


        // =====================================================
        // PASAR VELOCIDAD AL OBJETO
        // =====================================================

        FallingItem fallingItem =
            newItem.GetComponent<FallingItem>();


        if (fallingItem != null)
        {
            fallingItem.fallSpeed = fallSpeed;
        }


        Debug.Log(
            "Objeto creado: " +
            newItem.name +
            " | X: " +
            spawnPosition.x +
            " | Velocidad: " +
            fallSpeed
        );
    }


    // =========================================================
    // ELEGIR OBJETO
    // =========================================================

    private GameObject GetRandomItem()
    {
        float random = Random.value;


        // POWER UP
        if (random < powerUpChance)
        {
            if (powerUps != null &&
                powerUps.Length > 0)
            {
                return powerUps[
                    Random.Range(0, powerUps.Length)
                ];
            }
        }


        // OBJETO MALO
        if (random < powerUpChance + negativeChance)
        {
            if (negativeItems != null &&
                negativeItems.Length > 0)
            {
                return negativeItems[
                    Random.Range(0, negativeItems.Length)
                ];
            }
        }


        // OBJETO BUENO
        if (positiveItems != null &&
            positiveItems.Length > 0)
        {
            return positiveItems[
                Random.Range(0, positiveItems.Length)
            ];
        }


        return null;
    }


    // =========================================================
    // CAMBIAR DIFICULTAD
    // =========================================================

    public void SetDifficulty(
        float newSpawnInterval,
        float newFallSpeed,
        float newNegativeChance)
    {
        spawnInterval = newSpawnInterval;
        fallSpeed = newFallSpeed;
        negativeChance = newNegativeChance;
    }
}