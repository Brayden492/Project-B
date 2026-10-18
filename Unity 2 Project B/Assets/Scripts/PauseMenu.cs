using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject optionsScreen;

    void Update()
    {
        // checking for if the escape key gets pressed, if it does do another check for if the game is paused already or not
        // if the game is already paused then resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1f)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    // sets my canvas element active with all the pause screen items
    // sets time scale to 0 to stop any game actions from happening
    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;

        // because it is first person, when the game is paused the cursor needs to be unlocked and visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // when resuming just do the opposite of pause
    // hide pause screen and set time scale back to 1
    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;

        // then relock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // loads the main menu
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // this is for accessing options while in the game
    // now it isnt needed to return to the main menu for the options screen
    public void OpenOptions()
    {
        pauseScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    // this is just closing the options screen
    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }
}

