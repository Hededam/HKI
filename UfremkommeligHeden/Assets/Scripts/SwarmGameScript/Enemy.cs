using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f; // Speed of the enemy

    private Transform playerTransform; // Player's transform

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Move towards the player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
