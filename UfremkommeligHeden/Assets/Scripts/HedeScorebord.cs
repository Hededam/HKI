using UnityEngine;
using TMPro;

public class HedeScorebord : MonoBehaviour
{
    public TMP_Text[] highscoreTexts; // UI text fields for the highscores
    public TMP_Text currentXpText; // UI text field for the current XP
    public TMP_InputField playerNameInput; // Input field for the player's name

    private PlayerXp playerXp; // Player's XP
    private bool hasSavedHighscore = false; // Tjekker om highscoren allerede er gemt
    private string secretCode = "mads er gud"; // Hemmelig kode for at nulstille scorebordet

    void Start()
    {
        
    
        // Begræns antallet af bogstaver til 10  
        playerNameInput.characterLimit = 20;
        // Find the GameObject with the tag "Gamestuff"
        GameObject gamestuff = GameObject.FindGameObjectWithTag("Gamestuff");

        // Get the PlayerXp component from the GameObject
        playerXp = gamestuff.GetComponent<PlayerXp>();

        UpdateHighscoreTexts();
        // Update the current XP text with the player's XP
        currentXpText.text = "Du fik samlet : " + playerXp.xp.ToString() + " XP sammen";
    }

    void UpdateHighscoreTexts()
    {
        // Load the highscores and update the highscore texts
        for (int i = 0; i < highscoreTexts.Length; i++)
        {
            int highscore = PlayerPrefs.GetInt("Highscore" + i, 0);
            string playerName = PlayerPrefs.GetString("PlayerName" + i, "Unknown");
            highscoreTexts[i].text = "Highscore " + (i + 1) + ": " + highscore + " Sat af " + playerName;
        }
    }

    public void SaveHighscore()
    {
        // Tjekker om den indtastede tekst er lig med den hemmelige kode
        if (playerNameInput.text.Equals(secretCode))
        {
            ResetHighscores();
        }
        else if (!hasSavedHighscore)
        {
            int playerXP = playerXp.xp;
            string playerName = playerNameInput.text;

            for (int i = 0; i < highscoreTexts.Length; i++)
            {
                int highscore = PlayerPrefs.GetInt("Highscore" + i, 0);

                if (playerXP > highscore)
                {
                    // Opdaterer highscores
                    for (int j = highscoreTexts.Length - 1; j > i; j--)
                    {
                        PlayerPrefs.SetInt("Highscore" + j, PlayerPrefs.GetInt("Highscore" + (j - 1), 0));
                        PlayerPrefs.SetString("PlayerName" + j, PlayerPrefs.GetString("PlayerName" + (j - 1), "Unknown"));
                        highscoreTexts[j].color = Color.white; // Gør de gamle highscores hvide
                    }

                    PlayerPrefs.SetInt("Highscore" + i, playerXP);
                    PlayerPrefs.SetString("PlayerName" + i, playerName);
                    highscoreTexts[i].color = Color.red; // Gør den nye highscore rød

                    // Opdaterer highscoreTexts med de nye værdier
                    UpdateHighscoreTexts();
                    break;
                }
            }

            // Gemmer ændringerne i PlayerPrefs
            PlayerPrefs.Save();
            hasSavedHighscore = true; // Sætter hasSavedHighscore til true, så highscoren ikke kan gemmes igen
        }
    }


    // Metode til at nulstille alle highscores
    private void ResetHighscores()
    {
        for (int i = 0; i < highscoreTexts.Length; i++)
        {
            PlayerPrefs.SetInt("Highscore" + i, 0);
            PlayerPrefs.SetString("PlayerName" + i, "Unknown");
        }

        // Opdaterer highscoreTexts med de nulstillede værdier
        UpdateHighscoreTexts();

        // Gemmer ændringerne i PlayerPrefs
        PlayerPrefs.Save();
        hasSavedHighscore = false; // Tillader highscores at blive gemt igen
        playerNameInput.text = ""; // Nulstiller inputfeltet
    }

    public void ClearPlayerNameInput()
    {
        playerNameInput.text = ""; // Sætter tekstfeltet til en tom streng
    }
}
