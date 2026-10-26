using UnityEngine;

public class RoomsTracker : MonoBehaviour
{
    int enemiesRemaining;
    UpgradeManager upgradeManager;

    public bool RoomIsCleared { get; private set; }

    public bool UpgradeChosen { get; private set; }

    public int ClearedRooms { get; private set; } = 0;

    // Find the uprgade manager
    void Awake()
    {
        upgradeManager = Object.FindFirstObjectByType<UpgradeManager>();
    }
    
    // Sets enemy count and sets progress to not cleared
    public void SetEnemyCount(int count)
    {
        enemiesRemaining = count;
        //Debug.Log("Enemies counted for this room: " + enemiesRemaining);
        RoomIsCleared = false;
        UpgradeChosen = false;

        // Checks for when all enemies die
        if (enemiesRemaining <= 0)
            RoomCleared();
    }

    //Updates enemy count when an enemy dies
    public void EnemyDied()
    {
        // Prevents the count from changing after room has been cleared
        if (RoomIsCleared)
            return;

        enemiesRemaining--;
        //Debug.Log("Enemy died, enemies remaining: " + enemiesRemaining);

        // Also checking for when all enemies have died
        if (enemiesRemaining <= 0)
        {
            enemiesRemaining = 0;
            RoomCleared();
        }
    }

    // Marks room as cleared, increment room count, and display upgrade menu
    void RoomCleared()
    {
        //Prevent room from being counter as cleared more than once
        if (RoomIsCleared)
            return;

        RoomIsCleared = true;

        ClearedRooms++;
        //Debug.Log("Rooms cleared: " + ClearedRooms);

        //Debug.Log("Room cleared, all enemies are defeated");

        // Finding the upgrade menu
        if (upgradeManager == null)
            upgradeManager = Object.FindFirstObjectByType<UpgradeManager>();

        // Show the choices
        if (upgradeManager != null)
            upgradeManager.ShowUpgradePanel();
        else
            Debug.LogError("UpgradeManager was not found");
    }

    // Checks for when the player has chosen an upgrade
    public void MarkUpgradeChosen()
    {
        UpgradeChosen = true;
    }
}
