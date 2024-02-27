using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderHede : MonoBehaviour
{
    public string newSceneName; // Navnet på den nye scene, der skal loades
    public string oldSceneName; // Navnet på den scene, der skal unloades
    public string objectToDestroy; // Navnet på GameObjectet, der skal slettes

    public void OnButtonClick()
    {
        // Gem den nuværende aktive scene
        Scene currentScene = SceneManager.GetSceneByName(oldSceneName);

        // Load den nye scene additivt
        SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);

        // Flyt spilleren til den nye placering
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find spillerobjektet ved hjælp af tagget
        GameObject teleportLocation = GameObject.Find("EmptyTeleportLocation"); // Find teleporteringsplaceringen i den nye scene
        player.transform.position = teleportLocation.transform.position;

        // Unload den gamle scene
        SceneManager.UnloadSceneAsync(currentScene);

        // Slet det specificerede GameObject, hvis det eksisterer
        GameObject objectToDestroyInstance = GameObject.Find(objectToDestroy);
        if (objectToDestroyInstance != null)
        {
            Destroy(objectToDestroyInstance);
        }
    }
}
