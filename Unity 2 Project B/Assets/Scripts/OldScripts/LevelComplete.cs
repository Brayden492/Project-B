using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject failedScreen;
    [SerializeField] int totalEnemies = 3;
    [SerializeField] int requiredKills = 3;
    [SerializeField] TMPro.TMP_Text victoryKillResultText;
    [SerializeField] TMPro.TMP_Text failedKillResultText;

    private void OnTriggerEnter(Collider other)
    {
        // checking for if the player collided with this object
        if (other.CompareTag("Player"))
        {
            // setting the text for failed screen and victory screen
            victoryKillResultText.text = "You managed to kill " + GlobalKillCount.killCount +
                " out of " + totalEnemies + " enemies.";

            failedKillResultText.text = "You managed to kill " + GlobalKillCount.killCount +
                " out of " + totalEnemies + " enemies.";

            // now checking if the player got enough kills
            if (GlobalKillCount.killCount >= requiredKills)
            {
                // enough kills means victory
                victoryScreen.SetActive(true);
            }
            else
            {
                // not enough kills means failure
                failedScreen.SetActive(true);
            }

            // whether the player failed or succeeded, the game is completed
            // so time scale goes to 0 and game complete is true
            Time.timeScale = 0f;

            GameComplete.gameComplete = true;

            // then also unlock the cursor and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
