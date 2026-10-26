using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject victoryPanel;

    bool gameOver;

    // Making sure that the screens will be off when game starts
    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    // Ends the run when player triggers lose condition and displays the game over screen
    public void EndRun()
    {
        if (gameOver)
            return;

        gameOver = true;
        GameComplete.gameComplete = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        else
            Debug.LogError("GameOverPanel is not assigned");
        
        // This freezes the game to mimic pausing, and unlocks cursor
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Debug.Log("Run ended, Game over.");
    }

    // Same idea as previous, end run when player triggers win condition and displays victory screen
    public void WinRun()
    {
        if (gameOver)
            return;

        gameOver = true;
        GameComplete.gameComplete = true;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        else
            Debug.LogError("VictoryPanel is not assigned");

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Debug.Log("Boss defeated, You win");
    }

    // As name suggests, it takes player back to main menu screen
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        GameComplete.gameComplete = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
