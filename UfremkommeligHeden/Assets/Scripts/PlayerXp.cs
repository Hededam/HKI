using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerXp : MonoBehaviour
{
    public int xp = 0; // Player's XP
    public int health = 100; // Player's health
    public float PlayTimeLeft = 1000; // Change this to float
    public Text xpText; // UI text field for the XP
    public Text healthText; // UI text field for the health
    public Text PlayTimeLeftText; // UI text field for the PlayTimeLeft
    public Image damageImage; // UI image for the damage effect
    public string Himlen;

    void Start()
    {

        // Set the alpha value of the damage image to 1
        damageImage.color = new Color(1, 0, 0, 1);

        // Start the fade effect
        StartCoroutine(ShowDamageEffectCoroutine());
    }

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
            GameOver();
        }
        if (health <= 0)
        {
            Debug.Log("Du har taget for meget skade og er død");
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
                     ShowDamageEffect();
        }
    }

    private IEnumerator ShowDamageEffectCoroutine()
    {
        // Change the color of the damage image to red and reset the alpha value to 1
        damageImage.color = new Color(1, 0, 0, 1);

        // Fade the damage image back to transparent over time
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            // Interpolate the alpha value from 1 to 0 over one second
            float alpha = Mathf.Lerp(1, 0, t);
            damageImage.color = new Color(1, 0, 0, alpha);
            yield return null;
        }

        // Ensure the alpha value is set to 0 at the end of the fade
        damageImage.color = new Color(1, 0, 0, 0);
    }


    public void ShowDamageEffect()
    {
        // Stop the previous coroutine if it's still running
        StopCoroutine("ShowDamageEffectCoroutine");

        // Start a new coroutine to show the damage effect
        StartCoroutine("ShowDamageEffectCoroutine");
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

    private void GameOver()
    {
        // Ikke mere spille tid:
        Debug.Log("Player has died! Din tid er forbi sorry");
        SceneManager.LoadScene(Himlen);
    }
}
