using UnityEngine;

public class RoomExit : MonoBehaviour
{
    RoomsTracker roomsTracker;
    RoomGenerator roomGenerator;

    bool playerEntered;

    // Find room tracker and room generator
    void Start()
    {
        roomsTracker =
            Object.FindFirstObjectByType<RoomsTracker>();

        roomGenerator =
            Object.FindFirstObjectByType<RoomGenerator>();
    }

    // Checks if the player is elligible to use the exit
    void OnTriggerEnter(Collider other)
    {
        // Ignore anything not apart of player
        if (!other.transform.root.CompareTag("Player"))
            return;

        // Prevent same exit from being used multiple times 
        if (playerEntered)
            return;

        if (roomsTracker == null || roomGenerator == null)
            return;

        // player must kill all enemies
        if (!roomsTracker.RoomIsCleared)
        {
            Debug.Log("Defeat all enemies before leaving");
            return;
        }

        // player must select an upgrade
        if (!roomsTracker.UpgradeChosen)
        {
            Debug.Log("Choose an upgrade before leaving");
            return;
        }

        // Exit was used generate next room
        playerEntered = true;
        roomGenerator.GenerateNextRoom();
    }
}
