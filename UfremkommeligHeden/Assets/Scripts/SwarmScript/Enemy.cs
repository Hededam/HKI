using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f; // Fjendens hastighed
    public int damage = 1; // M�ngden af skade, fjenden p�f�rer
    public int xpReward = 20; // M�ngden af XP, der bel�nnes, n�r fjenden �del�gges
    public float chasePlayerDistance = 10f; // Afstanden, hvor fjenden begynder at jage spilleren

    private Transform targetTransform; // M�lets transform

    private void Start()
    {
        FindClosestEndpoint();
    }

    private void Update()
    {
        // Bev�g dig mod m�let
        Vector3 direction = (targetTransform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Hvis spilleren er inden for en bestemt afstand, begynd at jage spilleren
        Transform playerTransform = GameObject.Find("Player").transform;
        if (Vector3.Distance(transform.position, playerTransform.position) <= chasePlayerDistance)
        {
            targetTransform = playerTransform;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Endpoint") || collision.gameObject.CompareTag("Player"))
        {
            // M�let tager skade
            Life life = collision.gameObject.GetComponent<Life>();
            if (life != null)
            {
                life.TakeDamage(damage); // Brug TakeDamage-metoden fra Life
            }

            // Hvis fjenden kolliderer med spilleren, tager spilleren skade
            PlayerXp playerXp = collision.gameObject.GetComponent<PlayerXp>();
            if (playerXp != null && collision.gameObject.CompareTag("Player"))
            {

                playerXp.TakeDamage(damage); // Brug TakeDamage-metoden fra PlayerXp
            }
             Debug.Log("fjenden kolliderer med spilleren, completed ");
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        // H�ndter fjendens �del�ggelse (f.eks. afspil partikeleffekt, fjern fra scenen)
        // Bel�n spilleren med XP
        PlayerXp playerXp = GameObject.Find("Player").GetComponent<PlayerXp>();
        if (playerXp != null)
        {
            Debug.Log("DestroyEnemy() completed: " );
            playerXp.GainXP(xpReward);
        }

        Destroy(gameObject); // Fjern fjendeobjektet
    }

    private void FindClosestEndpoint()
    {
        GameObject[] endpoints = GameObject.FindGameObjectsWithTag("Endpoint");
        float closestDistance = Mathf.Infinity;
        GameObject closestEndpoint = null;

        foreach (GameObject endpoint in endpoints)
        {
            float distance = Vector3.Distance(transform.position, endpoint.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEndpoint = endpoint;
            }
        }

        if (closestEndpoint != null)
        {
            targetTransform = closestEndpoint.transform;
        }
    }
}
