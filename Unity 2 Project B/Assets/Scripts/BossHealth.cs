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

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("Boss defeated!");

            GameOverManager gameOverManager =
                Object.FindFirstObjectByType<GameOverManager>();

            if (gameOverManager != null)
            {
                gameOverManager.WinRun();
            }
            else
            {
                Debug.LogError("GameOverManager was not found!");
            }

            Destroy(gameObject);
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.fillAmount = (float)currentHealth / maxHealth;

        if (healthText != null)
            healthText.text = currentHealth.ToString();
    }
}
