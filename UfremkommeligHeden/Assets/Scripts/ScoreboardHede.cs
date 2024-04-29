using UnityEngine;
using TMPro;

public class ScoreboardHede : MonoBehaviour
{
    public TMP_Text xpText; // UI text field for the XP
    public TMP_Text highscoreText; // UI text field for the highscore
    public TMP_Text healthText; // UI text field for the Health
    public TMP_Text playTimeLeftText; // UI text field for the PlayTimeLeft

    private PlayerXp player; // Reference to the PlayerXp script
    private int highestXP = 0; // Variable to store the highest XP

    void Start()
    {
        // Find the player in the scene using the tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerXp>();
        }
        else
        {
            Debug.Log("Player object not found.");
        }

        // Load the highest XP from PlayerPrefs (if it exists)
        highestXP = PlayerPrefs.GetInt("HighestXP", 0);
    }

    void Update()
    {
        if (player != null)
        {
            // Update the UI text fields with the player's stats
            xpText.text = "XP: " + player.xp;
            healthText.text = "Health: " + player.health;
            playTimeLeftText.text = "Time Left: " + FormatTime(player.PlayTimeLeft);

            // Check if the current XP is higher than the stored highestXP
            if (player.xp > highestXP)
            {
                highestXP = player.xp;
                SaveHighestXP(); // Save the new highest XP
            }

            // Update the highscore text field
            highscoreText.text = "Highscore: " + highestXP;
        }
    }

    string FormatTime(float timeInSeconds)
    {
        // Convert the time to minutes and seconds
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds - minutes * 60);

        // Return the time in MM:SS format
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void SaveHighestXP()
    {
        // Save the highest XP to PlayerPrefs
        PlayerPrefs.SetInt("HighestXP", highestXP);
        PlayerPrefs.Save();
    }
}
