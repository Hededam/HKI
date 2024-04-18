using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    GravityAttractor closestPlanet;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Find the closest planet
        GravityAttractor[] allPlanets = FindObjectsOfType<GravityAttractor>();
        float closestDistance = Mathf.Infinity;

        foreach (GravityAttractor planet in allPlanets)
        {
            float distance = Vector3.Distance(rb.position, planet.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlanet = planet;
            }
        }

        // Apply the gravity of the closest planet
        if (closestPlanet != null)
        {
            closestPlanet.Attract(rb);
        }
    }
}
