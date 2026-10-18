using UnityEngine;

public class HealPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerHealing playerHealing =
            other.GetComponentInParent<PlayerHealing>();

        if (playerHealing != null)
        {
            playerHealing.AddHeal();
            Destroy(gameObject);
        }
    }

}
