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


    // Finds the room tracker
    void Start()
    {
        roomsTracker = Object.FindFirstObjectByType<RoomsTracker>();
    }

    // Spawns everything into the room
    public void SpawnRoomContents(RoomData roomData)
    {
        // Finds room tracker and room generator
        if (roomsTracker == null)
        {
            roomsTracker = Object.FindFirstObjectByType<RoomsTracker>();
        }

        if (roomGenerator == null)
        {
            roomGenerator = Object.FindFirstObjectByType<RoomGenerator>();
        }

        // Spawn the player
        SpawnPlayer(roomData);

        // Spawn boss 
        if (roomGenerator != null && roomGenerator.IsBossRoom)
        {
            SpawnBoss(roomData);
            return;
        }
        //Spawn regular enemies and potion pickup
        SpawnEnemies(roomData);
        SpawnPotion(roomData);
    }

    //Spawns the player into the first room and makes sure the same player gets carried over between different rooms.
    void SpawnPlayer(RoomData roomData)
    {
        // Is spawnpoint valid
        if (roomData.playerSpawnPoint == null)
        {
            Debug.LogWarning("Player spawn point is missing");
            return;
        }

        // Create player if needed
        if (playerInstance == null)
        {
            if (playerPrefab == null)
            {
                Debug.LogWarning("Player prefab is missing");
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
            // This handles moving the player instead of spawning a new one
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

    // Spawn enemies in their potential spawn locations
    void SpawnEnemies(RoomData roomData)
    {
        // Checking that everything is found and connected
        if (enemyPrefabs == null || enemyPrefabs.Length == 0 ||
            roomData.enemySpawnPoints == null ||
            roomData.enemySpawnPoints.Length == 0)
        {
            Debug.LogWarning("Enemy prefabs or spawn points are missing");
            return;
        }

        // Looking at max and min amount of enemies
        int maxPossible = Mathf.Min(
            roomData.maxEnemies,
            roomData.enemySpawnPoints.Length
        );

        int minPossible = Mathf.Clamp(
            roomData.minEnemies,
            0,
            maxPossible
        );

        // Random amount of enemies
        int enemyCount = Random.Range(minPossible, maxPossible + 1);

        // Make a copy so it wont change the original spawn points
        Transform[] availablePoints =
            (Transform[])roomData.enemySpawnPoints.Clone();

        int spawnedCount = 0;

        // Spawn enemy at a possible spawn location while also makign sure no same spawn point gets used twice
        for (int i = 0; i < enemyCount; i++)
        {
            int pointIndex = Random.Range(i, availablePoints.Length);

            Transform temp = availablePoints[i];
            availablePoints[i] = availablePoints[pointIndex];
            availablePoints[pointIndex] = temp;

            int enemyIndex = Random.Range(0, enemyPrefabs.Length);

            // Skip if prefab or spawn point is missing
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

        // Telling room tracker how many enemies have spawned
        if (roomsTracker != null)
            roomsTracker.SetEnemyCount(spawnedCount);
    }

    // Spawns boss at first spawn location that was set
    void SpawnBoss(RoomData roomData)
    {
        //Has boss prefab been set
        if (bossPrefab == null)
        {
            Debug.LogError("Boss prefab is not assigned");
            return;
        }

        // Is spawn valid
        if (roomData.enemySpawnPoints == null ||
            roomData.enemySpawnPoints.Length == 0 ||
            roomData.enemySpawnPoints[0] == null)
        {
            Debug.LogError("Boss spawn point is missing");
            return;
        }

        Instantiate(
            bossPrefab,
            roomData.enemySpawnPoints[0].position,
            roomData.enemySpawnPoints[0].rotation
        );
    }


    // Handles potion spawm
    void SpawnPotion(RoomData roomData)
    {

        // Check for prefab
        if (potionPrefab == null || roomData.pickupSpawnPoint == null)
        {
            Debug.LogWarning("Potion prefab or spawn point is missing");
            return;
        }

        Instantiate(
            potionPrefab,
            roomData.pickupSpawnPoint.position,
            roomData.pickupSpawnPoint.rotation
        );
    }
}
