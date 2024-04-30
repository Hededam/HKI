using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 10; // Mængden af skade, der påføres spilleren

    private void OnCollisionEnter(Collision collision)
    {
        // Tjek om kollisionen er med spilleren
        if (collision.gameObject.CompareTag("Player"))
        {
            // Find GameObject med tagget "Gamestuff"
            GameObject gameStuff = GameObject.FindGameObjectWithTag("Gamestuff");

            // Hvis GameObject med tagget "Gamestuff" eksisterer, hent PlayerXp-komponenten
            if (gameStuff != null)
            {
                PlayerXp playerXp = gameStuff.GetComponent<PlayerXp>();

                // Hvis PlayerXp-komponenten eksisterer, påfør skade
                if (playerXp != null)
                {
                    Debug.Log("TakeDamage");
                    playerXp.TakeDamage(damageAmount);
                }
            }
        }
    }
}
