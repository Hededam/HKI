using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal2021SceneSwitcher : MonoBehaviour
{
    public string sceneToUnload; // Navnet p� scenen, der skal unloades
    public string sceneToLoad; // Navnet p� scenen, der skal loades

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Tjekker om det er spilleren, der er g�et ind i triggeren
        {
            SceneManager.UnloadSceneAsync(sceneToUnload);
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);

            // Opdaterer lastLoadedScene i SceneLoaderHede scriptet
            SceneLoaderHede.lastLoadedScene = sceneToLoad;
        }
    }
}