using UnityEngine;

public class RoomExit : MonoBehaviour
{
    RoomsTracker roomsTracker;
    RoomGenerator roomGenerator;

    bool playerEntered;

    void Start()
    {
        roomsTracker =
            Object.FindFirstObjectByType<RoomsTracker>();

        roomGenerator =
            Object.FindFirstObjectByType<RoomGenerator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        if (playerEntered)
            return;

        if (roomsTracker == null || roomGenerator == null)
            return;

        if (!roomsTracker.RoomIsCleared)
        {
            Debug.Log("Defeat all enemies before leaving!");
            return;
        }

        if (!roomsTracker.UpgradeChosen)
        {
            Debug.Log("Choose an upgrade before leaving!");
            return;
        }

        playerEntered = true;
        roomGenerator.GenerateNextRoom();
    }
}
