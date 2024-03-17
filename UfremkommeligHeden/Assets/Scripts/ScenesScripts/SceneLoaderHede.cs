using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System.Linq;

public class SceneLoaderHede : MonoBehaviour
{
    public string TheBackgroundScene; // Navnet på scenen, der altid skal være aktiv
    public string[] scenesToUnload; // Array med navne på scener, der skal afmelde
    public string newSceneName; // Navnet på den scene, der skal indlæses
    public static string lastLoadedScene = "TutorialScenen"; // Navnet på den seneste indlæste scene

    // Reference til dit TMP textfelt (til at vise den seneste indlæste scene)
    public TMP_Text sceneNameText;

    private void Start()
    {
        // Opdater tekstfeltet med den seneste indlæste scene ved spilstart
        sceneNameText.text = lastLoadedScene;
    }

    public void OnButtonClick()
    {
        if (!string.IsNullOrEmpty(newSceneName) && newSceneName != lastLoadedScene)
        {
            StartCoroutine(LoadSceneWithDelay(newSceneName));
        }
    }

    private IEnumerator LoadSceneWithDelay(string newSceneName)
    {
        // Aflæs den gamle scene
        UnloadScenes(scenesToUnload);

        // Indlæs den nye scene
        Debug.Log("Loading scene: " + newSceneName);
        SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive).completed += SceneLoadCompleted;

        lastLoadedScene = newSceneName;

        // Opdater tekstfeltet med den seneste indlæste scene
        sceneNameText.text = lastLoadedScene;

        yield return null;
    }

    private void SceneLoadCompleted(AsyncOperation operation)
    {
        Debug.Log("Scene load completed: " + lastLoadedScene);
    }

    private void UnloadScenes(string[] scenesToUnload)
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
