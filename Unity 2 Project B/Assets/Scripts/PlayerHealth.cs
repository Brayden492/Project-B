using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth = 100;

    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text healthText;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar == null)
        {
            GameObject bar = GameObject.Find("Health");

            if (bar != null)
                healthBar = bar.GetComponent<Image>();
        }

        if (healthText == null)
        {
            GameObject text = GameObject.Find("HealthNumber");

            if (text != null)
                healthText = text.GetComponent<TMP_Text>();
        }

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("Player has died!");
        }
    }

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

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthUI();
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsAtFullHealth()
    {
        return currentHealth >= maxHealth;
    }

}

