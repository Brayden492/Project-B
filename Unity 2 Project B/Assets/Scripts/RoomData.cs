using UnityEngine;


// bunch of room data as name suggests
public class RoomData : MonoBehaviour
{
    // how many enemies can spawn
    [Header("Enemy Count")]
    public int minEnemies = 1;
    public int maxEnemies = 3;

    // where does player spawn
    public Transform playerSpawnPoint;

    //where can the enemies spawn
    public Transform[] enemySpawnPoints;
    
    //where does the potion spawn
    public Transform pickupSpawnPoint;
}
