using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderHede : MonoBehaviour
{
    public string newSceneName;
    public string oldSceneName;
    public GameObject player;
    public GameObject teleportLocation;
    public string sceneToUnloadNext;

    public void OnButtonClick()
    {
        if (!string.IsNullOrEmpty(newSceneName))
        {
            Debug.Log("Loading scene: " + newSceneName); // Debug meddelelse for at indikere at scenen indlæses
            SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);

            if (player != null && teleportLocation != null)
            {
                Debug.Log("Teleporting player to: " + teleportLocation.name); // Debug meddelelse for at indikere spillerens teleportering
                player.transform.position = teleportLocation.transform.position;
            }

            sceneToUnloadNext = newSceneName;
            UnloadNextScene();
        }
    }

    public void UnloadNextScene()
    {
        string sceneToUnload = string.IsNullOrEmpty(oldSceneName) ? sceneToUnloadNext : oldSceneName;

        if (!string.IsNullOrEmpty(sceneToUnload))
        {
            Debug.Log("Unloading scene: " + sceneToUnload); // Debug meddelelse for at indikere at scenen unloades
            SceneManager.UnloadSceneAsync(sceneToUnload);

            oldSceneName = string.Empty;
        }
    }
}
