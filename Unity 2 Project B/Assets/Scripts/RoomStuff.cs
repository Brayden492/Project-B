using UnityEngine;

public class RoomStuff : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSpawnPoint;

    [Header("Enemies")]
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform[] enemySpawnPoints;

    [SerializeField] int minEnemies = 1;
    [SerializeField] int maxEnemies = 3;

    [Header("Potion")]
    [SerializeField] GameObject potionPrefab;
    [SerializeField] Transform potionSpawnPoint;

    void Start()
    {
        SpawnPlayer();
        SpawnEnemies();
        SpawnPotion();
    }

    void SpawnPlayer()
    {
        if (playerPrefab == null || playerSpawnPoint == null)
        {
            Debug.LogWarning("Player prefab or spawn point is missing!");
            return;
        }

        Instantiate(
            playerPrefab,
            playerSpawnPoint.position,
            playerSpawnPoint.rotation
        );
    }

    void SpawnEnemies()
    {
        if (enemyPrefabs.Length == 0 || enemySpawnPoints.Length == 0)
        {
            Debug.LogWarning("Enemy prefabs or spawn points are missing!");
            return;
        }

        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);

        enemyCount = Mathf.Min(enemyCount, enemySpawnPoints.Length);

        for (int i = 0; i < enemyCount; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);

            Instantiate(
                enemyPrefabs[randomIndex],
                enemySpawnPoints[i].position,
                enemySpawnPoints[i].rotation
            );
        }
    }

    void SpawnPotion()
    {
        if (potionPrefab == null || potionSpawnPoint == null)
        {
            Debug.LogWarning("Potion prefab or spawn point is missing!");
            return;
        }

        Instantiate(
            potionPrefab,
            potionSpawnPoint.position,
            potionSpawnPoint.rotation
        );
    }
}
