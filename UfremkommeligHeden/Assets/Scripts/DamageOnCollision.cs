using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    public int damage = 1; // Skaden, der påføres spilleren ved kollision

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hvis objektet kolliderer med spilleren, tager spilleren skade
            PlayerXp playerXp = collision.gameObject.GetComponent<PlayerXp>();
            if (playerXp != null)
            {
                playerXp.TakeDamage(damage); // Brug TakeDamage-metoden fra PlayerXp
            }
        }
    }
}
