using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusGravity : MonoBehaviour
{
    public Transform torusObject; // Reference til torusobjektet
    public float gravity = 9.8f; // Gravitationskonstant
    public float totusRadius = 60;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Find den nærmeste position på torusobjektets overflade
        Vector3 closestPoint = GetClosestPointOnTorus(transform.position);

        // Beregn retningen mellem spilleren og målpunktet
        Vector3 gravityDirection = (closestPoint - transform.position).normalized;

        // Anvend tyngdekraften på spillerens Rigidbody
        rb.AddForce(gravityDirection * gravity, ForceMode.Acceleration);
    }

    private Vector3 GetClosestPointOnTorus(Vector3 point)
    {
        Vector3 center = torusObject.position;
        Vector3 normal = (point - center).normalized;
        return center + normal * GetTorusRadius();
    }

    private float GetTorusRadius()
    {
        // Tilpas denne funktion for at returnere den korrekte radius for din torus
        return totusRadius;
    }
}