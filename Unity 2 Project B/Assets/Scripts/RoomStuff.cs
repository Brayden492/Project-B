using UnityEditor.EditorTools;
using UnityEngine;

public class RoomStuff : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject playerPrefab;

    [Header("Enemies")]
    [SerializeField] GameObject[] enemyPrefabs;

    [Header("Potion")]
    [SerializeField] GameObject potionPrefab;

    [Header("Boss")]
    [SerializeField] GameObject bossPrefab;

    RoomGenerator roomGenerator;
    RoomsTracker roomsTracker;
    GameObject playerInstance;

    void Start()
    {
        roomsTracker = Object.FindFirstObjectByType<RoomsTracker>();
    }

    public void SpawnRoomContents(RoomData roomData)
    {
        if (roomsTracker == null)
        {
            roomsTracker = Object.FindFirstObjectByType<RoomsTracker>();
        }

        if (roomGenerator == null)
        {
            roomGenerator = Object.FindFirstObjectByType<RoomGenerator>();
        }

        SpawnPlayer(roomData);

        if (roomGenerator != null && roomGenerator.IsBossRoom)
        {
            SpawnBoss(roomData);
            return;
        }
        SpawnEnemies(roomData);
        SpawnPotion(roomData);
    }

    void SpawnPlayer(RoomData roomData)
    {
        if (roomData.playerSpawnPoint == null)
        {
            Debug.LogWarning("Player spawn point is missing!");
            return;
        }

        if (playerInstance == null)
        {
            if (playerPrefab == null)
            {
                Debug.LogWarning("Player prefab is missing!");
                return;
            }

            playerInstance = Instantiate(
                playerPrefab,
                roomData.playerSpawnPoint.position,
                roomData.playerSpawnPoint.rotation
            );
        }
        else
        {
            CharacterController controller =
                playerInstance.GetComponent<CharacterController>();

            if (controller != null)
                controller.enabled = false;

            playerInstance.transform.SetPositionAndRotation(
                roomData.playerSpawnPoint.position,
                roomData.playerSpawnPoint.rotation
            );

            if (controller != null)
                controller.enabled = true;
        }
    }

    void SpawnEnemies(RoomData roomData)
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0 ||
            roomData.enemySpawnPoints == null ||
            roomData.enemySpawnPoints.Length == 0)
        {
            Debug.LogWarning("Enemy prefabs or spawn points are missing!");
            return;
        }

        int maxPossible = Mathf.Min(
            roomData.maxEnemies,
            roomData.enemySpawnPoints.Length
        );

        int minPossible = Mathf.Clamp(
            roomData.minEnemies,
            0,
            maxPossible
        );

        int enemyCount = Random.Range(minPossible, maxPossible + 1);

        // Make a copy so it wont modify the original spawn points.
        Transform[] availablePoints =
            (Transform[])roomData.enemySpawnPoints.Clone();

        int spawnedCount = 0;

        for (int i = 0; i < enemyCount; i++)
        {
            int pointIndex = Random.Range(i, availablePoints.Length);

            Transform temp = availablePoints[i];
            availablePoints[i] = availablePoints[pointIndex];
            availablePoints[pointIndex] = temp;

            int enemyIndex = Random.Range(0, enemyPrefabs.Length);

            if (enemyPrefabs[enemyIndex] == null ||
                availablePoints[i] == null)
            {
                continue;
            }

            Instantiate(
                enemyPrefabs[enemyIndex],
                availablePoints[i].position,
                availablePoints[i].rotation
            );

            spawnedCount++;
        }

        if (roomsTracker != null)
            roomsTracker.SetEnemyCount(spawnedCount);
    }

    void SpawnBoss(RoomData roomData)
    {
        if (bossPrefab == null)
        {
            Debug.LogError("Boss prefab is not assigned!");
            return;
        }

        if (roomData.enemySpawnPoints == null ||
            roomData.enemySpawnPoints.Length == 0 ||
            roomData.enemySpawnPoints[0] == null)
        {
            Debug.LogError("Boss spawn point is missing!");
            return;
        }

        Instantiate(
            bossPrefab,
            roomData.enemySpawnPoints[0].position,
            roomData.enemySpawnPoints[0].rotation
        );
    }

    void SpawnPotion(RoomData roomData)
    {
        if (potionPrefab == null || roomData.pickupSpawnPoint == null)
        {
            Debug.LogWarning("Potion prefab or spawn point is missing!");
            return;
        }

        Instantiate(
            potionPrefab,
            roomData.pickupSpawnPoint.position,
            roomData.pickupSpawnPoint.rotation
        );
    }
}
