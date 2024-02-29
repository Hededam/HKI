using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderHede : MonoBehaviour
{
    public string newSceneName; // Navnet p� den nye scene, der skal loades
    public string oldSceneName; // Navnet p� den scene, der skal unloades

    // Referencer til GameObjects
    public GameObject player; // S�t dette i inspekt�ren
    public GameObject teleportLocation; // S�t dette i inspekt�ren

    public string sceneToUnloadNext; // Variabel til at gemme navnet p� den n�ste scene, der skal unloades

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

            // Gem navnet p� den n�ste scene, der skal unloades
            sceneToUnloadNext = newSceneName;
        }
    }

    // Kald denne metode fra et andet sted i dit script, n�r du �nsker at unload'e den n�ste scene
    public void UnloadNextScene()
    {
        string sceneToUnload = string.IsNullOrEmpty(oldSceneName) ? sceneToUnloadNext : oldSceneName;

        if (!string.IsNullOrEmpty(sceneToUnload))
        {
            // Unload den n�ste scene ved hj�lp af det gemte navn
            SceneManager.UnloadSceneAsync(sceneToUnload);

            // Nulstil oldSceneName
            oldSceneName = string.Empty;
        }
    }
}
