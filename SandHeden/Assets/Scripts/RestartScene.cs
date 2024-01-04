using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public Button restartButton; // Reference til knappen i Unity-editoren

    void Start()
    {
        restartButton.onClick.AddListener(RestartCurrentScene);
    }

    public void RestartCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
