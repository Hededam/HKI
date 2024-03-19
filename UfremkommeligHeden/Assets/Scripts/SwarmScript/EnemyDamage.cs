using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 10; // M�ngden af skade, der p�f�res spilleren

    private void OnCollisionEnter(Collision collision)
    {
        // Tjek om kollisionen er med spilleren
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hent PlayerXp-komponenten fra spilleren
            PlayerXp playerXp = collision.gameObject.GetComponent<PlayerXp>();

            // Hvis PlayerXp-komponenten eksisterer, p�f�r skade
            if (playerXp != null)
            {
                Debug.Log("TakeDamage");
                playerXp.TakeDamage(damageAmount);
            }
        }
    }
}
