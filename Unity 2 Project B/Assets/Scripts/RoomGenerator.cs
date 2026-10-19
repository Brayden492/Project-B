using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] roomPrefabs;
    [SerializeField] RoomStuff roomStuff;

    GameObject currentRoomObject;
    RoomsTracker roomsTracker;
    bool generatingRoom;

    public RoomData CurrentRoom { get; private set; }

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
        if (roomPrefabs == null || roomPrefabs.Length == 0)
        {
            Debug.LogError("No room prefabs assigned!");
            return;
        }

        int randomIndex = Random.Range(0, roomPrefabs.Length);

        currentRoomObject = Instantiate(
            roomPrefabs[randomIndex],
            Vector3.zero,
            Quaternion.identity
        );

        CurrentRoom = currentRoomObject.GetComponent<RoomData>();

        if (CurrentRoom == null)
        {
            Debug.LogError("Selected room is missing RoomData!");
            return;
        }

        Debug.Log("Generated room: " + currentRoomObject.name);

        if (roomStuff == null)
            roomStuff = Object.FindFirstObjectByType<RoomStuff>();

        if (roomStuff != null)
        {
            roomStuff.SpawnRoomContents(CurrentRoom);
        }
        else
        {
            Debug.LogError("RoomStuff was not found!");
        }
    }
}
