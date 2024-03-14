using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f; // Fjendens hastighed
    public int damage = 10; // Mængden af skade, fjenden påfører
    public int xpReward = 20; // Mængden af XP, der belønnes, når fjenden ødelægges

    private Transform playerTransform; // Spillerens transform

    private void Start()
    {
        playerTransform = GameObject.Find("PlayerController").transform;
    }

    private void Update()
    {
        // Bevæg dig mod spilleren
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Spilleren tager skade
            PlayerXp playerXp = collision.gameObject.GetComponent<PlayerXp>();
            if (playerXp != null)
            {
                playerXp.TakeDamage(damage); // Brug TakeDamage-metoden fra PlayerXp
            }
        }
    }

    public void DestroyEnemy()
    {
        // Håndter fjendens ødelæggelse (f.eks. afspil partikeleffekt, fjern fra scenen)
        // Beløn spilleren med XP
        PlayerXp playerXp = GameObject.Find("PlayerController").GetComponent<PlayerXp>();
        if (playerXp != null)
        {
            playerXp.GainXP(xpReward);
        }

        Destroy(gameObject); // Fjern fjendeobjektet
    }
}
