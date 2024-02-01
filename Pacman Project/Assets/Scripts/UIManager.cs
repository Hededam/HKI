using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public GameManager GameManager;

    public TextMeshProUGUI livesText;
    public string livesMessage = "Lives: ";

    public TextMeshProUGUI scoreText;
    public string scoreMessage = "Score: ";
  
    public void UpdateLivesUI(int amount)
    {
        livesText.text = livesMessage + amount;
    }

    public void UpdateScoreUI(int amount)
    {
        scoreText.text = scoreMessage + amount;
    }


}
