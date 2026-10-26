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

    // Find room tracker
    void Awake()
    {
        roomsTracker = Object.FindFirstObjectByType<RoomsTracker>();
    }

    // Generate the first room
    void Start()
    {
        GenerateRoom();
    }

    // Get rid of old room and generate next one
    public void GenerateNextRoom()
    {
        // Make sure only one room is generated
        if (generatingRoom)
            return;

        generatingRoom = true;

        //Remove old room
        if (currentRoomObject != null)
            Destroy(currentRoomObject);

        GenerateRoom();

        generatingRoom = false;
    }

    // Generates rooms
    public void GenerateRoom()
    {
        // Find room tracker
        if (roomsTracker == null)
            roomsTracker = Object.FindFirstObjectByType<RoomsTracker>();

        // Has the player cleared enough rooms
        bool shouldSpawnBoss =
            roomsTracker != null &&
            roomsTracker.ClearedRooms >= roomsBeforeBoss;

        GameObject prefabToSpawn;

        // If yes generate boss room
        if (shouldSpawnBoss)
        {
            if (bossRoomPrefab == null)
            {
                Debug.LogError("Boss room prefab is not assigned");
                return;
            }

            prefabToSpawn = bossRoomPrefab;
            IsBossRoom = true;
        }
        else
        {
            // If no then generate a normal room
            if (roomPrefabs == null || roomPrefabs.Length == 0)
            {
                Debug.LogError("No room prefabs assigned");
                return;
            }

            int randomIndex = Random.Range(0, roomPrefabs.Length);
            prefabToSpawn = roomPrefabs[randomIndex];
            IsBossRoom = false;
        }

        //Generate the map at the origin and with no rotation added
        currentRoomObject = Instantiate(
            prefabToSpawn,
            Vector3.zero,
            Quaternion.identity
        );
        
        // Getting the room data
        CurrentRoom = currentRoomObject.GetComponent<RoomData>();

        if (CurrentRoom == null)
        {
            Debug.LogError("Selected room is missing RoomData");
            return;
        }

        //Debug.Log(
        //    IsBossRoom
        //        ? "Generated boss room!"
        //        : "Generated regular room: " + currentRoomObject.name
        //);

        // Spawning stuff
        if (roomStuff == null)
            roomStuff = Object.FindFirstObjectByType<RoomStuff>();

        if (roomStuff != null)
            roomStuff.SpawnRoomContents(CurrentRoom);
        else
            Debug.LogError("RoomStuff was not found");
    }
}
