using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public string scoreMessage = " Score";

    public void Setup(int score) {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " Score";
    }

    public void RestartButton() {
        SceneManager.LoadScene("SampleScene");
    }
}
