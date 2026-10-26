using UnityEngine;
using TMPro;

public class PlayerHealing : MonoBehaviour
{
    [Header("Healing")]
    [SerializeField] int heals = 1;
    [SerializeField, Range(0f, 1f)]
    float healPercentage = 0.30f;

    [Header("UI")]
    [SerializeField] TMP_Text healText;

    PlayerHealth playerHealth;

    // Finds player health and connects the ui
    void Start()
    {
        // Get healing counter
        playerHealth = GetComponent<PlayerHealth>();
        if (healText == null)
        {
            GameObject text = GameObject.Find("HealAmount");

            if (text != null)
                healText = text.GetComponent<TMP_Text>();
        }
        // Display starting number of heals
        UpdateHealUI();
    }

    // Press Q to heal
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseHeal();
        }
    }

    // Used 1 heal to restor a percentage of health
    void UseHeal()
    {
        // if no heals then nothing happens
        if (heals <= 0 || playerHealth == null)
            return;

        // Prevent heals from being wasted
        if (playerHealth.IsAtFullHealth())
            return;

        // Heal amount
        int healAmount = Mathf.CeilToInt(
            playerHealth.GetMaxHealth() * healPercentage
        );

        // Deduct 1 heal and heal the player
        heals--;
        playerHealth.Heal(healAmount);
        UpdateHealUI();
    }

    // Give the player another heal when getting the potion pickup
    public void AddHeal()
    {
        heals++;
        UpdateHealUI();
    }

    // Update ui to show how many heals player has
    void UpdateHealUI()
    {
        if (healText != null)
        {
            healText.text = "Heals: " + heals;
        }
    }
}
