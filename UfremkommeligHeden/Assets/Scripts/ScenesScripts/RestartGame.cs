using UnityEngine;
using UnityEngine.SceneManagement;



public class RestartGame : MonoBehaviour
{
    private static RestartGame instance;

private void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        // Hvis der allerede findes en GameManager, s� fjern denne
        Destroy(gameObject);
    }
}

// Kald denne funktion for at genstarte scenen og fjerne DontDestroyOnLoad-objektet
public void RestartAll()
{
    SceneManager.LoadScene(0); // Inds�t scenens indeks, som du �nsker at genstarte (0 er normalt startscenen)
    Destroy(gameObject); // Fjern dette objekt fra scenen
}
}