using UnityEngine;

public class RemoveByDistance : MonoBehaviour
{
    [SerializeField] private float maxDistance = 2000f; // Maksimal afstand fra referencepunkt

    private Vector3 referencePoint; // Referencepunkt (f.eks. Vektor3(0,0,0))

    private void Start()
    {
        referencePoint = Vector3.zero; // Indstil referencepunkt til (0,0,0) ved start
    }

    private void Update()
    {
        // Gennemgå alle objekter i scenen
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            // Beregn afstanden mellem objektet og referencepunktet
            float distance = Vector3.Distance(obj.transform.position, referencePoint);

            // Hvis afstanden er større end den specificerede maksimale afstand, fjern objektet
            if (distance > maxDistance)
            {
                Destroy(obj);
            }
        }
    }
}
