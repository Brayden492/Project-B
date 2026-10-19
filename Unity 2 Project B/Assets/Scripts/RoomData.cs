using UnityEngine;

public class RoomData : MonoBehaviour
{
    [Header("Enemy Count")]
    public int minEnemies = 1;
    public int maxEnemies = 3;
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;
    public Transform pickupSpawnPoint;
}
