using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] GameObject upgradePanel;

    bool upgradeChosen;

    // Make sure upgrade panel isnt visivle on start
    void Start()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }

    // Mimic pausing the game and show the upgrade screen
    public void ShowUpgradePanel()
    {
        // check if upgrade panel is assigned or not
        if (upgradePanel == null)
        {
            Debug.LogError("UpgradePanel is not assigned");
            return;
        }

        // Let player choose an upgrade
        upgradeChosen = false;
        upgradePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    // Damage upgrade stuff
    public void ChooseDamageUpgrade()
    {
        // Stop player from getting multiple upgrades
        if (upgradeChosen)
            return;

        //Find the gun
        HandGunFire gun =
            Object.FindFirstObjectByType<HandGunFire>();

        if (gun == null)
        {
            Debug.LogError("HandGunFire was not found");
            return;
        }

        gun.UpgradeDamage();
        FinishUpgrade();
    }

    // Do health upgrade stuff
    public void ChooseHealthUpgrade()
    {
        if (upgradeChosen)
            return;

        // Find player and the players health
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        PlayerHealth health = player != null
            ? player.GetComponentInChildren<PlayerHealth>()
            : null;

        if (health == null)
        {
            Debug.LogError("PlayerHealth was not found");
            return;
        }

        health.UpgradeMaxHealth(25);
        FinishUpgrade();
    }

    // Firerate upgrade stuf
    public void ChooseFireRateUpgrade()
    {
        if (upgradeChosen)
            return;

        // Also needs to find the gun
        HandGunFire gun =
            Object.FindFirstObjectByType<HandGunFire>();

        if (gun == null)
        {
            Debug.LogError("HandGunFire was not found");
            return;
        }

        gun.UpgradeFireRate();
        FinishUpgrade();
    }

    // Finishes upgrade, updates room progress and resume game
    void FinishUpgrade()
    {
        // Telling room tracker that an upgrade has been selected
        upgradeChosen = true;

        RoomsTracker tracker =
            Object.FindFirstObjectByType<RoomsTracker>();

        if (tracker != null)
            tracker.MarkUpgradeChosen();

        // close the menu and resume
        upgradePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        //Debug.Log("Upgrade selected");
    }
}
