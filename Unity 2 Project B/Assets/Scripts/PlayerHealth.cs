using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth = 100;

    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text healthText;

    // Initializes player health and connects it to ui
    void Start()
    {
        currentHealth = maxHealth;

        // Find the health bar
        if (healthBar == null)
        {
            GameObject bar = GameObject.Find("Health");

            if (bar != null)
                healthBar = bar.GetComponent<Image>();
        }

        // Find the health bar text
        if (healthText == null)
        {
            GameObject text = GameObject.Find("HealthNumber");

            if (text != null)
                healthText = text.GetComponent<TMP_Text>();
        }

        // Display starting health
        UpdateHealthUI();
    }

    // Increase max health while also providing some slight healing
    public void UpgradeMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        UpdateHealthUI();

        //Debug.Log("Max health upgraded to " + maxHealth);
    }

    // Handles taking damage and ending game if health depleted
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthUI();

        // Game over
        if (currentHealth <= 0)
        {
            //Debug.Log("Player has died");

            // Needs game over manager
            GameOverManager gameOverManager = Object.FindFirstObjectByType<GameOverManager>();

            if (gameOverManager != null) 
                gameOverManager.EndRun();
            else
                Debug.LogError("GameOverManager was not found");
        }
    }

    // Updating the health bar on the ui
    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }

    // Lets player use a heal wihout letting it go past the max health amount
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthUI();
    }

    // Capture the players max health 
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    // Check if player is full health or not
    public bool IsAtFullHealth()
    {
        return currentHealth >= maxHealth;
    }

}

