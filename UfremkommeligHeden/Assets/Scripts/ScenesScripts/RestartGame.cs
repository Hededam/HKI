using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public string sceneToLoad; // Navnet p� scenen, der skal loades

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Tjekker om det er spilleren, der er g�et ind i triggeren
        {
            DestroyGameStuffObject();
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void DestroyGameStuffObject()
    {
        GameObject gameStuffObject = GameObject.FindGameObjectWithTag("Gamestuff");
        if (gameStuffObject != null)
        {
            Destroy(gameStuffObject);
        }
    }
}
