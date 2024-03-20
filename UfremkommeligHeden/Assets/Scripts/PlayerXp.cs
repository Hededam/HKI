using UnityEngine;
using UnityEngine.UI;

public class PlayerXp : MonoBehaviour
{
    public int xp = 0; // Player's XP
    public int health = 100; // Player's health
    public float PlayTimeLeft = 1000; // Change this to float

    public Text xpText; // UI text field for the XP
    public Text healthText; // UI text field for the health
    public Text PlayTimeLeftText; // UI text field for the PlayTimeLeft
    public Image damageImage; // UI image for the damage effect

    private void Update()
    {
        // Decrease the PlayTimeLeft by the time passed since the last frame
        PlayTimeLeft -= Time.deltaTime;

        // Convert the PlayTimeLeft to minutes and seconds
        int minutes = Mathf.FloorToInt(PlayTimeLeft / 60F);
        int seconds = Mathf.FloorToInt(PlayTimeLeft - minutes * 60);

        // Update the UI text fields
        xpText.text = "XP: " + xp;
        healthText.text = "Health: " + health;
        PlayTimeLeftText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds); // Display the time left in MM:SS format

        // Check if the PlayTimeLeft has reached zero
        if (PlayTimeLeft <= 0)
        {
            Debug.Log("Game Over");
            Die();
        }
    }

    public void GainXP(int amount)
    {
        xp += amount;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player burde tage skade nu?");
        {
            Debug.Log("Player brude d� nu");
            Die();
            ShowDamageEffect();
        }
    }

    private void ShowDamageEffect()
    {
        // Change the color of the damage image to red
        damageImage.color = Color.red;

        // Fade the damage image back to transparent over time
        damageImage.CrossFadeAlpha(0f, 1f, false);
    }

    private void Die()
    {
        // Handle player death here
        Debug.Log("Player has died!");

        // Find the SceneStuff GameObject
        GameObject sceneStuffObject = GameObject.Find("SceneStuff");

        if (sceneStuffObject != null)
        {
            // Check if the object has the PlayerDeathHandler script attached
            PlayerDeathHandler playerDeathHandler = sceneStuffObject.GetComponent<PlayerDeathHandler>();

            if (playerDeathHandler != null)
            {
                // If the script is attached, execute the HandlePlayerDeath() function
                playerDeathHandler.HandlePlayerDeath();
                Debug.Log("HandlePlayerDeath() function executed successfully.");
            }
            else
            {
                Debug.Log("PlayerDeathHandler script not found on SceneStuff object.");
            }
        }
        else
        {
            Debug.Log("SceneStuff object not found.");
        }
    }
}
