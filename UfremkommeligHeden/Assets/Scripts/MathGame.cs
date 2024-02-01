using UnityEngine;
using TMPro;

public class MathGame : MonoBehaviour
{
    public TextMeshProUGUI problemText;
    public TMP_InputField answerInput; // Skiftet til TextMeshPro InputField
    public AudioClip correctAnswerSound;
    public GameObject newGameObjectPrefab;

    private int num1;
    private int num2;
    private int correctAnswer;

    void Start()
    {
        GenerateProblem();
    }

    void GenerateProblem()
    {
        num1 = Random.Range(1, 10);
        num2 = Random.Range(1, 10);
        correctAnswer = num1 + num2;

        problemText.text = $"{num1} + {num2} = ?";
    }

    public void CheckAnswer()
    {
        int playerAnswer;

        if (int.TryParse(answerInput.text, out playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                Debug.Log("Correct answer!");

                if (correctAnswerSound != null)
                {
                    AudioSource.PlayClipAtPoint(correctAnswerSound, transform.position);
                }

                if (newGameObjectPrefab != null)
                {
                    Instantiate(newGameObjectPrefab, transform.position, Quaternion.identity);
                }

                GenerateProblem();
            }
            else
            {
                Debug.Log("Wrong answer. Try again!");
            }

            answerInput.text = "";
        }
    }
}
