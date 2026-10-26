using UnityEngine;

public class GlobalKillCount : MonoBehaviour
{
    public static int killCount = 0;

    [SerializeField] TMPro.TMP_Text killDisplay;

    // set the count to zero
    void Start()
    {
        killCount = 0;
    }

    // keep updating the kill count that is on screen
    void Update()
    {
        killDisplay.text = "Kills: " + killCount;
    }
}
