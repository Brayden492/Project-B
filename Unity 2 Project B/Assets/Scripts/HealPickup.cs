using UnityEngine;

public class HealPickup : MonoBehaviour
{
    // Detecting when player touches the object
    void OnTriggerEnter(Collider other)
    {
        PlayerHealing playerHealing =
            other.GetComponentInParent<PlayerHealing>();

        // Gives the player one extra heal and gets rid of the object on the floor
        if (playerHealing != null)
        {
            playerHealing.AddHeal();
            Destroy(gameObject);
        }
    }

}
