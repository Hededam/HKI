using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f; // Fjendens hastighed
    public int damage = 10; // M�ngden af skade, fjenden p�f�rer
    public int xpReward = 20; // M�ngden af XP, der bel�nnes, n�r fjenden �del�gges

    private Transform playerTransform; // Spillerens transform

    private void Start()
    {
        playerTransform = GameObject.Find("PlayerController").transform;
    }

    private void Update()
    {
        // Bev�g dig mod spilleren
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
        // H�ndter fjendens �del�ggelse (f.eks. afspil partikeleffekt, fjern fra scenen)
        // Bel�n spilleren med XP
        PlayerXp playerXp = GameObject.Find("PlayerController").GetComponent<PlayerXp>();
        if (playerXp != null)
        {
            playerXp.GainXP(xpReward);
        }

        Destroy(gameObject); // Fjern fjendeobjektet
    }
}
