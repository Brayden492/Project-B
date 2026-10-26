using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 500;
    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text healthText;

    int currentHealth;
    bool isDead;

    // Initialize the health
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // Handles boss damage
    public void TakeDamage(int damage)
    {
        //Boss cant take damage after dying
        if (isDead)
            return;

        // Subtract health
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthUI();

        // Check for when health is depleted
        if (currentHealth <= 0)
        {
            isDead = true;
            //Debug.Log("Boss defeated");

            // Needs game over manager since the boss is the win condition
            GameOverManager gameOverManager =
                Object.FindFirstObjectByType<GameOverManager>();

            if (gameOverManager != null)
            {
                gameOverManager.WinRun();
            }
            else
            {
                Debug.LogError("GameOverManager was not found");
            }

            // Removes the boss
            Destroy(gameObject);
        }
    }

    // Uodating the health bar
    void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.fillAmount = (float)currentHealth / maxHealth;

        if (healthText != null)
            healthText.text = currentHealth.ToString();
    }
}
