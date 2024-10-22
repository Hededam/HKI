using BNG;
using UnityEngine;

public class RagdollDamageReceiver : MonoBehaviour
{
    private Damageable damageable;
    public float damageAmount = 10f; // Skaden der p�f�res
    public float minimumImpactSpeed = 2f; // Minimum hastighed for at p�f�re skade

    void Start()
    {
        // Find Damageable komponenten p� parent-objektet
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
