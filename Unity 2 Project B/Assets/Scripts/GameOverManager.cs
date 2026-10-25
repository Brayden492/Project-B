using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject victoryPanel;

    bool gameOver;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void EndRun()
    {
        if (gameOver)
            return;

        gameOver = true;
        GameComplete.gameComplete = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        else
            Debug.LogError("GameOverPanel is not assigned!");

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Run ended! Game over.");
    }

    public void WinRun()
    {
        if (gameOver)
            return;

        gameOver = true;
        GameComplete.gameComplete = true;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        else
            Debug.LogError("VictoryPanel is not assigned!");

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Boss defeated! You win!");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        GameComplete.gameComplete = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
