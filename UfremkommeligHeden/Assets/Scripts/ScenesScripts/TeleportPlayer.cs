using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    // Reference til det tomme GameObject, hvor spilleren skal teleporteres hen
    public Transform emptyTeleportLocation;

    private void Start()
    {
        // Teleportér spilleren til det tomme GameObject
        PlayerTeleport(emptyTeleportLocation.position);
    }

    private void PlayerTeleport(Vector3 targetPosition)
    {
        // Find spilleren (antager, at den har tagget \"PlayerController\")
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Flyt spilleren til targetPosition
        if (player != null)
        {
            player.transform.position = targetPosition;
        }
        else
        {
            Debug.LogError("Spilleren blev ikke fundet og kunne ikke Telepoteres!");
        }
    }
}
