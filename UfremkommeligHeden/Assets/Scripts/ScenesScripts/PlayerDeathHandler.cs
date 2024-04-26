using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathHandler : MonoBehaviour
{
    private PlayerXp playerXp; // Reference til PlayerXp-scriptet på spilleren
    public string sceneToLoad;
    public string sceneToUnload; // Navnet på scenen, der skal unloades
    public bool setPlayerHealthTo100 = false;
    public bool setPlayerXPToZero = false;
    public bool teleportToSpecificLocation = false;
    public Transform specificTeleportLocation; 
    public float RedusePlayTimeLeft = 0f;

    private void Start()
    {
        // Find PlayerXp-komponenten på "Player"-objektet
        playerXp = GameObject.Find("Player").GetComponent<PlayerXp>();
    }

    // Kald denne metode, når spilleren dør
    public void HandlePlayerDeath()
    {
        if (playerXp == null)
        {
            Debug.LogError("PlayerXp-komponent ikke fundet!");
            return;
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.UnloadSceneAsync(sceneToUnload);
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);

            // Opdaterer lastLoadedScene i SceneLoaderHede-scriptet
            SceneLoaderHede.lastLoadedScene = sceneToLoad;
        }

        if (setPlayerHealthTo100)
        {
            // Sæt spillerens sundhed til den specificerede værdi
            playerXp.health = 100;
        }

        if (setPlayerXPToZero)
        {
            // Sæt spillerens XP til 0
            playerXp.xp = 0;
        }

        if (teleportToSpecificLocation && specificTeleportLocation != null)
        {
            // Teleportér spilleren til det specifikke sted
            transform.position = specificTeleportLocation.position;
        }

        // Træk RedusePlayTimeLeft fra PlayTimeLeft
        playerXp.PlayTimeLeft -= RedusePlayTimeLeft;
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
