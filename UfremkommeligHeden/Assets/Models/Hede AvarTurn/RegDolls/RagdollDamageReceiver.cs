using BNG;
using UnityEngine;

public class RagdollDamageReceiver : MonoBehaviour
{
    private Damageable damageable;

    void Start()
    {
        // Automatically find the Damageable component on the parent object
        damageable = GetComponentInParent<Damageable>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Example: Apply damage on collision. Adjust as necessary for your game's logic.
        if (collision.gameObject.CompareTag("Projectile"))
        {
            damageable.DealDamage(10); // Apply 10 damage, adjust as needed
        }
    }
}