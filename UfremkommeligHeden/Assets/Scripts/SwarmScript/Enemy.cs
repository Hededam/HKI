using UnityEngine;
//Det her script er i stykker
public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public int damage = 1;
    public int xpReward = 20;
    public float chasePlayerDistance = 10f;

    private Transform targetTransform;
    private Animator animator; // Tilføj denne linje
    private static readonly int IsWalking = Animator.StringToHash("IsWalking"); // Tilføj denne linje
    private static readonly int IsDead = Animator.StringToHash("IsDead"); // Tilføj denne linje

    private void Start()
    {
        animator = GetComponent<Animator>(); // Tilføj denne linje
        FindClosestEndpoint();
    }

    private void Update()
    {
        Vector3 direction = (targetTransform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Få fjenden til at kigge i bevægelsesretningen
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);

        // Nulstil rotationen omkring X- og Z-akserne
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        // Hvis spilleren er inden for en bestemt afstand, begynd at jage spilleren
        Transform playerTransform = GameObject.Find("Player").transform;
        if (Vector3.Distance(transform.position, playerTransform.position) <= chasePlayerDistance)
        {
            targetTransform = playerTransform;
        }

        // Opdater animationen baseret på fjendens bevægelse
        if (speed > 0)
        {
            animator.SetBool(IsWalking, true);
        }
        else
        {
            animator.SetBool(IsWalking, false);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Endpoint") || collision.gameObject.CompareTag("Player"))
        {
            Life life = collision.gameObject.GetComponent<Life>();
            if (life != null)
            {
                life.TakeDamage(damage);
            }

            PlayerXp playerXp = collision.gameObject.GetComponent<PlayerXp>();
            if (playerXp != null && collision.gameObject.CompareTag("Player"))
            {
                playerXp.TakeDamage(damage);
            }
            Debug.Log("fjenden kolliderer med spilleren, completed ");
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        animator.SetBool(IsDead, true); // Tilføj denne linje
        Invoke("ActuallyDestroyEnemy", 4f); // Tilføj denne linje
    }

    public void ActuallyDestroyEnemy() // Tilføj denne funktion
    {
        PlayerXp playerXp = GameObject.Find("Player").GetComponent<PlayerXp>();
        if (playerXp != null)
        {
            Debug.Log("DestroyEnemy() completed: ");
            playerXp.GainXP(xpReward);
        }

        Destroy(gameObject);
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
