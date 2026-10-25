using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] roomPrefabs;
    [SerializeField] RoomStuff roomStuff;

    [Header("Boss Room")]
    [SerializeField] GameObject bossRoomPrefab;
    [SerializeField] int roomsBeforeBoss = 5;

    GameObject currentRoomObject;
    RoomsTracker roomsTracker;
    bool generatingRoom;

    public RoomData CurrentRoom { get; private set; }
    public bool IsBossRoom { get; private set; }

    void Awake()
    {
        roomsTracker = Object.FindFirstObjectByType<RoomsTracker>();
    }

    void Start()
    {
        GenerateRoom();
    }

    public void GenerateNextRoom()
    {
        if (generatingRoom)
            return;

        generatingRoom = true;

        if (currentRoomObject != null)
            Destroy(currentRoomObject);

        GenerateRoom();

        generatingRoom = false;
    }

    public void GenerateRoom()
    {
        if (roomsTracker == null)
            roomsTracker = Object.FindFirstObjectByType<RoomsTracker>();

        bool shouldSpawnBoss =
            roomsTracker != null &&
            roomsTracker.ClearedRooms >= roomsBeforeBoss;

        GameObject prefabToSpawn;

        if (shouldSpawnBoss)
        {
            if (bossRoomPrefab == null)
            {
                Debug.LogError("Boss room prefab is not assigned!");
                return;
            }

            prefabToSpawn = bossRoomPrefab;
            IsBossRoom = true;
        }
        else
        {
            if (roomPrefabs == null || roomPrefabs.Length == 0)
            {
                Debug.LogError("No room prefabs assigned!");
                return;
            }

            int randomIndex = Random.Range(0, roomPrefabs.Length);
            prefabToSpawn = roomPrefabs[randomIndex];
            IsBossRoom = false;
        }

        currentRoomObject = Instantiate(
            prefabToSpawn,
            Vector3.zero,
            Quaternion.identity
        );

        CurrentRoom = currentRoomObject.GetComponent<RoomData>();

        if (CurrentRoom == null)
        {
            Debug.LogError("Selected room is missing RoomData!");
            return;
        }

        Debug.Log(
            IsBossRoom
                ? "Generated boss room!"
                : "Generated regular room: " + currentRoomObject.name
        );

        if (roomStuff == null)
            roomStuff = Object.FindFirstObjectByType<RoomStuff>();

        if (roomStuff != null)
            roomStuff.SpawnRoomContents(CurrentRoom);
        else
            Debug.LogError("RoomStuff was not found!");
    }
}
