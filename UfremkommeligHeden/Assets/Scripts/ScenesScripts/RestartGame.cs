
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public string sceneToLoad; // Navnet på scenen, der skal loades

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Tjekker om det er spilleren, der er gået ind i triggeren
        {

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}