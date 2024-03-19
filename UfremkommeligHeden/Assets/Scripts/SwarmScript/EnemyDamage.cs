using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 10; // Mængden af skade, der påføres spilleren

    private void OnCollisionEnter(Collision collision)
    {
        // Tjek om kollisionen er med spilleren
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hent PlayerXp-komponenten fra spilleren
            PlayerXp playerXp = collision.gameObject.GetComponent<PlayerXp>();

            // Hvis PlayerXp-komponenten eksisterer, påfør skade
            if (playerXp != null)
            {
                Debug.Log("TakeDamage");
                playerXp.TakeDamage(damageAmount);
            }
        }
    }
}
