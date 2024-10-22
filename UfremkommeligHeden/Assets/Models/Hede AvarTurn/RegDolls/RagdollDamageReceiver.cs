using BNG;
using UnityEngine;

public class RagdollDamageReceiver : MonoBehaviour
{
    private Damageable damageable;
    public float damageAmount = 10f; // Skaden der påføres
    public float minimumImpactSpeed = 2f; // Minimum hastighed for at påføre skade

    void Start()
    {
        // Find Damageable komponenten på parent-objektet
        damageable = GetComponentInParent<Damageable>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Kontroller kollisionens hastighed
        if (collision.relativeVelocity.magnitude >= minimumImpactSpeed)
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name + ", Impact Speed: " + collision.relativeVelocity.magnitude);
            damageable.DealDamage(damageAmount);
        }
    }
}
