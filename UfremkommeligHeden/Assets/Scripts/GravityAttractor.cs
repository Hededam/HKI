using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = -9.8f;
    public float gravityRadius = 50f;

    public void Attract(Rigidbody playerRb)
    {
        Vector3 direction = (playerRb.position - transform.position).normalized;
        float distance = Vector3.Distance(playerRb.position, transform.position);

        // Tjek om spilleren er inden for tyngdekraftsradiusen
        if (distance < gravityRadius)
        {
            Vector3 gravityUp = direction * gravity;

            // Apply downwards gravity to body
            playerRb.AddForce(gravityUp);

            // Align body's up axis with the centre of planet
            Quaternion targetRotation = Quaternion.FromToRotation(playerRb.transform.up, direction) * playerRb.rotation;
            playerRb.MoveRotation(Quaternion.Slerp(playerRb.rotation, targetRotation, 50f * Time.deltaTime));

            // Deactivate Unity's standard gravity
            playerRb.useGravity = false;
        }
        else
        {
            // Activate Unity's standard gravity
            playerRb.useGravity = true;
        }
    }
}
