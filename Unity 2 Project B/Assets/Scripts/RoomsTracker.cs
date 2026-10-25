using UnityEngine;

public class RoomsTracker : MonoBehaviour
{
    int enemiesRemaining;
    UpgradeManager upgradeManager;

    public bool RoomIsCleared { get; private set; }

    public bool UpgradeChosen { get; private set; }

    public int ClearedRooms { get; private set; } = 0;

    void Awake()
    {
        upgradeManager = Object.FindFirstObjectByType<UpgradeManager>();
    }

    public void SetEnemyCount(int count)
    {
        enemiesRemaining = count;
        Debug.Log("Enemies counted for this room: " + enemiesRemaining);
        RoomIsCleared = false;
        UpgradeChosen = false;

        if (enemiesRemaining <= 0)
            RoomCleared();
    }

    public void EnemyDied()
    {
        if (RoomIsCleared)
            return;

        enemiesRemaining--;
        Debug.Log("Enemy died! Enemies remaining: " + enemiesRemaining);

        if (enemiesRemaining <= 0)
        {
            enemiesRemaining = 0;
            RoomCleared();
        }
    }

    void RoomCleared()
    {
        if (RoomIsCleared)
            return;

        RoomIsCleared = true;

        ClearedRooms++;
        Debug.Log("Rooms cleared: " + ClearedRooms);

        Debug.Log("Room cleared! All enemies are defeated.");

        if (upgradeManager == null)
            upgradeManager = Object.FindFirstObjectByType<UpgradeManager>();

        if (upgradeManager != null)
            upgradeManager.ShowUpgradePanel();
        else
            Debug.LogError("UpgradeManager was not found!");
    }

    public void MarkUpgradeChosen()
    {
        UpgradeChosen = true;
    }
}
