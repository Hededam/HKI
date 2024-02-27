using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneLoader : MonoBehaviour
{
    public string newSceneName; // Navnet p� den scene, der skal loades ved start

    // Start-metoden kaldes, n�r scriptet er initialiseret
    void Start()
    {
          // Load den specificerede scene
        SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);
    }
}







