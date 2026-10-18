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

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        if (healText == null)
        {
            GameObject text = GameObject.Find("HealAmount");

            if (text != null)
                healText = text.GetComponent<TMP_Text>();
        }
        UpdateHealUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseHeal();
        }
    }

    void UseHeal()
    {
        if (heals <= 0 || playerHealth == null)
            return;

        if (playerHealth.IsAtFullHealth())
            return;

        int healAmount = Mathf.CeilToInt(
            playerHealth.GetMaxHealth() * healPercentage
        );

        heals--;
        playerHealth.Heal(healAmount);
        UpdateHealUI();
    }

    public void AddHeal()
    {
        heals++;
        UpdateHealUI();
    }

    void UpdateHealUI()
    {
        if (healText != null)
        {
            healText.text = "Heals: " + heals;
        }
    }
}
