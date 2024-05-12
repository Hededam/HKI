using UnityEngine;
using TMPro;

public class HedeScorebord : MonoBehaviour
{
    public TMP_Text[] highscoreTexts; // UI text fields for the highscores
    public TMP_Text currentXpText; // UI text field for the current XP
    public TMP_InputField playerNameInput; // Input field for the player's name

    private PlayerXp playerXp; // Player's XP

    void Start()
    {
        // Find the GameObject with the tag "Gamestoff"
        GameObject gamestoff = GameObject.FindGameObjectWithTag("Gamestuff");

        // Get the PlayerXp component from the GameObject
        playerXp = gamestoff.GetComponent<PlayerXp>();

        // Load the highscores and update the highscore texts
        for (int i = 0; i < highscoreTexts.Length; i++)
        {
            int highscore = PlayerPrefs.GetInt("Highscore" + i, 0);
            string playerName = PlayerPrefs.GetString("PlayerName" + i, "Unknown");
            highscoreTexts[i].text = "Highscore " + (i + 1) + ": " + highscore + " by " + playerName;
        }

        // Update the current XP text with the player's XP
        currentXpText.text = "Current XP: " + playerXp.xp.ToString();
           Debug.Log("GainXP called. Current XP: " + playerXp.xp);
    }

    public void SaveHighscore()
    {
        // Get the player's name from the input field
        string playerName = playerNameInput.text;

        // Check if the player's XP is higher than any of the saved highscores
        for (int i = 0; i < highscoreTexts.Length; i++)
        {
            int highscore = PlayerPrefs.GetInt("Highscore" + i, 0);

            if (playerXp.xp > highscore)
            {
                // If the player's XP is higher than a saved highscore, update the highscores
                for (int j = highscoreTexts.Length - 1; j > i; j--)
                {
                    PlayerPrefs.SetInt("Highscore" + j, PlayerPrefs.GetInt("Highscore" + (j - 1), 0));
                    PlayerPrefs.SetString("PlayerName" + j, PlayerPrefs.GetString("PlayerName" + (j - 1), "Unknown"));
                }

                PlayerPrefs.SetInt("Highscore" + i, playerXp.xp);
                PlayerPrefs.SetString("PlayerName" + i, playerName);
                break;
            }
        }

        // Save the changes to PlayerPrefs
        PlayerPrefs.Save();
    }
}
