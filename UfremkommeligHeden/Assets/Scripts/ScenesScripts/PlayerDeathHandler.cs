using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathHandler : MonoBehaviour
{
    // Reference to the PlayerXp script on the player
    private PlayerXp playerXp;

    // Handling for when the player dies
    public string sceneToLoad;
    public string sceneToUnload; // Navnet på scenen, der skal unloades
    public bool setPlayerHealthTo100 = false;
    public bool setPlayerXPToZero = false;
    public bool teleportToSpecificLocation = false;
    public Transform specificTeleportLocation; // Reference to the teleport location gameobject

    private void Start()
    {
        // Find the PlayerXp component on the "Player" gameobject
        playerXp = GameObject.Find("Player").GetComponent<PlayerXp>();

        // Initialize any necessary components or variables
    }

    // Call this method when the player dies
    public void HandlePlayerDeath()
    {
        if (playerXp == null)
        {
            Debug.LogError("PlayerXp component not found!");
            return;
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.UnloadSceneAsync(sceneToUnload);
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);

            // Opdaterer lastLoadedScene i SceneLoaderHede scriptet
            SceneLoaderHede.lastLoadedScene = sceneToLoad;
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

    private IEnumerator LoadSceneAfterUnloadingOthers(string sceneToLoad)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != "TheBackgroundScene")
            {
                yield return SceneManager.UnloadSceneAsync(scene);
            }
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
