using UnityEngine;

public class RoomsTracker : MonoBehaviour
{
    int enemiesRemaining;

    public bool RoomIsCleared { get; private set; }

    public void SetEnemyCount(int count)
    {
        enemiesRemaining = count;
        RoomIsCleared = false;

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
    }
}
