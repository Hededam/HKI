using UnityEngine;
using UnityEngine.UI;

public class PlayerXp : MonoBehaviour
{
    public int xp = 0; // Player's XP
    public int health = 100; // Player's health

    public Text xpText; // UI text field for the XP
    public Text healthText; // UI text field for the health

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

        }
    }

    private void Die()
    {
        // Handle player death here
        Debug.Log("Player has died!"); // Example: You can add more logic or effects here
    }
}
