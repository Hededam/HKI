using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    // Reference to the PlayerXp script on the player
    private PlayerXp playerXp;

    // Handling for when the player dies
    public bool reloadScene = true;
    public bool setPlayerHealthTo100 = false;
    public bool setPlayerXPToZero = false;
    public bool teleportToSpecificLocation = false;
    public Transform specificTeleportLocation; // Reference to the teleport location gameobject

    private void Start()
    {
        {
            // Find the PlayerXp component on the "Player" gameobject
            playerXp = GameObject.Find("Player").GetComponent<PlayerXp>();

            // Initialize any necessary components or variables
        }

    }


    // Call this method when the player dies
    public void HandlePlayerDeath()
    {
        if (playerXp == null)
        {
            Debug.LogError("PlayerXp component not found!");
            return;
        }

  
        if (reloadScene)
        {
            // Reload the current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (setPlayerHealthTo100)
        {
            // Set player health to the specified value
            playerXp.health = 100;
        }

        if (setPlayerXPToZero)
        {
            // Set player XP to 0
            playerXp.xp = 0;
        }

        if (teleportToSpecificLocation && specificTeleportLocation != null)
        {
            // Teleport the player to the specific location
            transform.position = specificTeleportLocation.position;
        }
    }
}
