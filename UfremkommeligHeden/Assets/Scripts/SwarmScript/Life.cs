using UnityEngine;
using UnityEngine.SceneManagement; // Tilføj denne linje
using TMPro; // Tilføj denne linje for at bruge TextMeshPro
using System.Collections.Generic; // Tilføj denne linje for at bruge List

public class Life : MonoBehaviour
{
    public int life = 100; // Livet for objektet
    public string SceneToLoad; // Navnet på den scene, der skal indlæses
    public List<string> scenesToUnload; // Liste af scener, der skal unloades
    public TextMeshProUGUI lifeText; // Reference til dit TMP tekstfelt

    private void Start()
    {
        UpdateLifeText();
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        UpdateLifeText();

        if (life <= 0)
        {
            // Håndter objektets død (f.eks. slut spillet, vis game over skærm)
            if (gameObject.CompareTag("Endpoint"))
            {
                SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Additive);

                // Opdaterer lastLoadedScene i SceneLoaderHede scriptet
                SceneLoaderHede.lastLoadedScene = SceneToLoad;

                foreach (string scene in scenesToUnload)
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
        }
    }

    private void UpdateLifeText()
    {
        if (lifeText != null)
        {
            lifeText.text = "Liv: " + life.ToString();
        }
    }
}
