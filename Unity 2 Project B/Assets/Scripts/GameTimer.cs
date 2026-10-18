using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] float timeRemaining = 60f;
    [SerializeField] GameObject failedScreen;
    [SerializeField] TMP_Text timerText;

    void Update()
    {
        // my timer
        timeRemaining -= Time.deltaTime;

        // puts the time on the screen for the player
        timerText.text = Mathf.Ceil(timeRemaining).ToString();

        // checking for when timer hits 0, then runs LoseGame
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            LoseGame();
        }
    }

    // turns on the failed screen canvas element I made for failing from running out of time
    void LoseGame()
    {
        failedScreen.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

