using UnityEngine;

public class RoomsTracker : MonoBehaviour
{
    int enemiesRemaining;
    UpgradeManager upgradeManager;

    public bool RoomIsCleared { get; private set; }

    public bool UpgradeChosen { get; private set; }

    void Awake()
    {
        upgradeManager = Object.FindFirstObjectByType<UpgradeManager>();
    }

    public void SetEnemyCount(int count)
    {
        enemiesRemaining = count;
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
