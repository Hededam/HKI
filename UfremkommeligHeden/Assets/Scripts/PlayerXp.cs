using UnityEngine;
using UnityEngine.UI;

public class PlayerXp : MonoBehaviour
{
    public int xp = 0; // Player's XP
    public int health = 100; // Player's health
    public int PlayTimeLeft;

    public Text xpText; // UI text field for the XP
    public Text healthText; // UI text field for the health
    public Text PlayTimeLeftText;
    public Image damageImage; // UI image for the damage effect

    private void Update()
    {
        // Update the UI text fields
        xpText.text = "XP: " + xp;
        healthText.text = "Health: " + health;
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
            Debug.Log("Player brude dø nu");
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
