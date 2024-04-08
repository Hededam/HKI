using UnityEngine;
using UnityEngine.SceneManagement; // Tilf�j denne linje
using TMPro; // Tilf�j denne linje for at bruge TextMeshPro
using System.Collections.Generic; // Tilf�j denne linje for at bruge List

public class Life : MonoBehaviour
{
    public int life = 100; // Livet for objektet
    public string SceneToLoad; // Navnet p� den scene, der skal indl�ses
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
            // H�ndter objektets d�d (f.eks. slut spillet, vis game over sk�rm)
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
