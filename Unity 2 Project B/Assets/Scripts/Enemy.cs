using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] int maxHealth = 100;
    [SerializeField] Image healthBar;
    [SerializeField] bool isDead = false;
    public TMP_Text healthText;

    void Start()
    {
        healthText.text = health.ToString();
    }

    public void TakeDamage(int damage)
    {
        // check if enemy is already dead or not
        if (isDead) return;
        
        // damage happens in the handgun fire script
        health -= damage;

        // health number value 
        healthText.text = health.ToString();

        // health bar changes with the health amount
        healthBar.fillAmount = (float)health / maxHealth;

        // when enemy dies set Dead to true and destroy the game object to make it disappear 
        // also update the killcount, only increment by one
        if (health <= 0)
        {
            RoomsTracker roomsTracker = Object.FindFirstObjectByType<RoomsTracker>();

            if (roomsTracker != null)
                roomsTracker.EnemyDied();
            isDead = true;
            GlobalKillCount.killCount++;
            Destroy(gameObject);
        }
    }
}
