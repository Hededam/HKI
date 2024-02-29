using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderHede : MonoBehaviour
{
    public string newSceneName; // Navnet på den nye scene, der skal loades
    public string oldSceneName; // Navnet på den scene, der skal unloades

    // Referencer til GameObjects
    public GameObject player; // Sæt dette i inspektøren
    public GameObject teleportLocation; // Sæt dette i inspektøren

    public string sceneToUnloadNext; // Variabel til at gemme navnet på den næste scene, der skal unloades

    public void OnButtonClick()
    {
        // Tjek om scene-navnet er gyldigt
        if (!string.IsNullOrEmpty(newSceneName))
        {
            // Load den nye scene additivt
            SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);

            // Flyt spilleren til den nye placering
            if (player != null && teleportLocation != null)
            {
                player.transform.position = teleportLocation.transform.position;
            }

            // Gem navnet på den næste scene, der skal unloades
            sceneToUnloadNext = newSceneName;
        }
    }

    // Kald denne metode fra et andet sted i dit script, når du ønsker at unload'e den næste scene
    public void UnloadNextScene()
    {
        string sceneToUnload = string.IsNullOrEmpty(oldSceneName) ? sceneToUnloadNext : oldSceneName;

        if (!string.IsNullOrEmpty(sceneToUnload))
        {
            // Unload den næste scene ved hjælp af det gemte navn
            SceneManager.UnloadSceneAsync(sceneToUnload);

            // Nulstil oldSceneName
            oldSceneName = string.Empty;
        }
    }
}
