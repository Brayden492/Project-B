using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // sets the time scale to 1 because in some places I set it to 0, so
    // whenever the player clicks start it needs to reset it to 1
    // the same thing applies for setting game complete to false
    // then I load the game scene
    public void StartGame()
    {
        Time.timeScale = 1f;
        GameComplete.gameComplete = false;

        SceneManager.LoadScene("Game");
    }

    // this gets used to return back to the main menu
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        GameComplete.gameComplete = false;
        SceneManager.LoadScene("MainMenu");
    }
}