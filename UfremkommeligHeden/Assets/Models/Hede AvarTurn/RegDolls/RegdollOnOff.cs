using UnityEngine;

public class RegdollOnOff : MonoBehaviour
{
    public Animation animationComponent;
    public Rigidbody[] ragdollRigidbodies;
    public Collider[] ragdollColliders;
    public bool startWithRagdollActive = false; 

    void Start()
    {
        SetRagdollActive(startWithRagdollActive);
    }

    public void SetRagdollActive(bool active)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !active;
        }

        foreach (Collider col in ragdollColliders)
        {
            col.enabled = active;
        }
    }

    public void TakeDamage()
    {
        animationComponent.enabled = false;
        SetRagdollActive(true);
    }
}
