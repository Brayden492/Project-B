using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] GameObject upgradePanel;

    bool upgradeChosen;

    void Start()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }

    public void ShowUpgradePanel()
    {
        if (upgradePanel == null)
        {
            Debug.LogError("UpgradePanel is not assigned!");
            return;
        }

        upgradeChosen = false;
        upgradePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void ChooseDamageUpgrade()
    {
        if (upgradeChosen)
            return;

        HandGunFire gun =
            Object.FindFirstObjectByType<HandGunFire>();

        if (gun == null)
        {
            Debug.LogError("HandGunFire was not found!");
            return;
        }

        gun.UpgradeDamage();
        FinishUpgrade();
    }

    public void ChooseHealthUpgrade()
    {
        if (upgradeChosen)
            return;

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        PlayerHealth health = player != null
            ? player.GetComponentInChildren<PlayerHealth>()
            : null;

        if (health == null)
        {
            Debug.LogError("PlayerHealth was not found!");
            return;
        }

        health.UpgradeMaxHealth(25);
        FinishUpgrade();
    }

    public void ChooseFireRateUpgrade()
    {
        if (upgradeChosen)
            return;

        HandGunFire gun =
            Object.FindFirstObjectByType<HandGunFire>();

        if (gun == null)
        {
            Debug.LogError("HandGunFire was not found!");
            return;
        }

        gun.UpgradeFireRate();
        FinishUpgrade();
    }

    void FinishUpgrade()
    {
        upgradeChosen = true;

        RoomsTracker tracker =
            Object.FindFirstObjectByType<RoomsTracker>();

        if (tracker != null)
            tracker.MarkUpgradeChosen();

        upgradePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        Debug.Log("Upgrade selected!");
    }
}
