using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SceneLoaderHede : MonoBehaviour
{
    public string newSceneName; // Den scene der skal loades
    public string TheBackgroundScene; // Den scene der altid skal være aktiv
    public string[] scenesToUnload; // Array til at specificere scener, der skal unloades

    public void OnButtonClick()
    {
        if (!string.IsNullOrEmpty(newSceneName))
        {
            Debug.Log("Loading scene: " + newSceneName);
            SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);

            UnloadScenesExcept(scenesToUnload);
        }
    }

    private void UnloadScenesExcept(string[] scenesToUnload)
    {
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scenesToUnload.Contains(scene.name) && scene.name != TheBackgroundScene)
            {
                Debug.Log("Unloading scene: " + scene.name);
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
}

