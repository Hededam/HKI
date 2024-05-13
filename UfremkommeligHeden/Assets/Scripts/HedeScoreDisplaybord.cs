using TMPro;
using UnityEngine;

public class HedeScoreDisplaybord : MonoBehaviour
{
    public TMP_Text AlltimehighscoreTexts; // UI text fields for the highscores
    public TMP_Text currentXpcoreText; // UI text field for the current XP
    public TMP_Text healthText; // UI text field for the player's health
    public TMP_Text timeLeftText; // UI text field for the time left

    private PlayerXp playerXp; // Reference to the PlayerXp script

    void Start()
    {
        // Find the GameObject with the tag "Gamestuff"
        GameObject gamestuff = GameObject.FindGameObjectWithTag("Gamestuff");

        // Get the PlayerXp component from the GameObject
        playerXp = gamestuff.GetComponent<PlayerXp>();

        // Check if the components are found
        if (playerXp == null)
        {
            Debug.LogError("PlayerXp component not found on the Gamestuff object.");
        }
    }

    private void Update()
    {
        // Check if the playerXp reference is not null
        if (playerXp != null)
        {
            // Update the UI text fields with the current values from PlayerXp script
            currentXpcoreText.text = "XP: " + playerXp.xp.ToString();
            healthText.text = "Health: " + playerXp.health.ToString();

            // Convert the PlayTimeLeft to minutes and seconds
            int minutes = Mathf.FloorToInt(playerXp.PlayTimeLeft / 60F);
            int seconds = Mathf.FloorToInt(playerXp.PlayTimeLeft - minutes * 60);

            // Update the time left in MM:SS format
            timeLeftText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Change text color to red if PlayTimeLeft is less than a minute
            if (playerXp.PlayTimeLeft < 60)
            {
                timeLeftText.color = Color.red;
            }
            else
            {
                // Set to default color if more than a minute is left
                timeLeftText.color = Color.white; // Eller en anden standardfarve efter dit valg
            }
        }
    }
}
