using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System.Linq;

public class SceneLoaderHede : MonoBehaviour
{
    public string TheBackgroundScene; // Navnet p� scenen, der altid skal v�re aktiv
    public string[] scenesToUnload; // Array med navne p� scener, der skal afmelde
    public string newSceneName; // Navnet p� den scene, der skal indl�ses
    public static string lastLoadedScene = "TutorialScenen"; // Navnet p� den seneste indl�ste scene

    // Reference til dit TMP textfelt (til at vise den seneste indl�ste scene)
    public TMP_Text sceneNameText;

    private void Start()
    {
        // Opdater tekstfeltet med den seneste indl�ste scene ved spilstart
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
        // Afl�s den gamle scene
        UnloadScenes(scenesToUnload);

        // Indl�s den nye scene
        Debug.Log("Loading scene: " + newSceneName);
        SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive).completed += SceneLoadCompleted;

        lastLoadedScene = newSceneName;

        // Opdater tekstfeltet med den seneste indl�ste scene
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
