using UnityEngine;
using TMPro;

public class MathGame : MonoBehaviour
{
    public TextMeshProUGUI problemText;
    public TMP_InputField answerInput;
    public AudioClip correctAnswerSound;
    public GameObject newGameObjectPrefab;
    public TextMeshProUGUI correctAnswersCountText;
    public TextMeshProUGUI difficultyLevelText; // Referencen til TMP text feltet

    public int correctAnswersCount { get; private set; }
    public int correctAnswersBeforeIncrease = 3; // Antal korrekte svar før sværhedsgraden øges
    public int difficultyLevel = 0; // Offentlig variabel for sværhedsgrad
    private int num1;
    private int num2;
    private int correctAnswer;
    private int correctAnswersSinceIncrease; // Antal korrekte svar siden sidste sværhedsgradsstigning
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GenerateProblem();
        UpdateCorrectAnswersCountText();
        UpdateDifficultyLevelText(); // Opdater TMP text feltet for sværhedsgrad ved start
    }

    void GenerateProblem()
    {
        switch (difficultyLevel)
        {
            case 0:
                num1 = Random.Range(1, 10);
                num2 = Random.Range(1, 10);
                break;
            case 1:
                num1 = Random.Range(10, 20);
                num2 = Random.Range(10, 20);
                break;
            case 2:
                num1 = Random.Range(20, 30);
                num2 = Random.Range(20, 30);
                break;
            case 3:
                num1 = Random.Range(30, 40);
                num2 = Random.Range(30, 40);
                break;
            case 4:
                num1 = Random.Range(40, 50);
                num2 = Random.Range(40, 50);
                break;
            case 5:
                num1 = Random.Range(50, 60);
                num2 = Random.Range(50, 60);
                break;
            case 6:
                num1 = Random.Range(100, 1000000);
                num2 = Random.Range(100, 1000000);
                break;
            // Tilføj flere niveauer efter behov
            default:
                num1 = Random.Range(1, 10);
                num2 = Random.Range(1, 10);
                break;
        }

        int operation = Random.Range(0, 4);
        switch (operation)
        {
            case 0:
                correctAnswer = num1 + num2;
                problemText.text = $"{num1} + {num2} = ?";
                break;
            case 1:
                if (num1 < num2)
                {
                    int temp = num1;
                    num1 = num2;
                    num2 = temp;
                }
                correctAnswer = num1 - num2;
                problemText.text = $"{num1} - {num2} = ?";
                break;
            case 2:
                correctAnswer = num1 * num2;
                problemText.text = $"{num1} * {num2} = ?";
                break;
            case 3:
                correctAnswer = Random.Range(2, 10);
                num2 = Random.Range(2, 10);
                num1 = correctAnswer * num2;
                problemText.text = $"{num1} ÷ {num2} = ?";
                break;
            default:
                Debug.LogError("Invalid operation!");
                break;
        }
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

                correctAnswersCount++;
                correctAnswersSinceIncrease++; // Øg antallet af korrekte svar siden sidste sværhedsgradsstigning
                UpdateCorrectAnswersCountText();

                if (correctAnswersSinceIncrease >= correctAnswersBeforeIncrease)
                {
                    IncreaseDifficulty();
                    correctAnswersSinceIncrease = 0; // Nulstil tælleren for korrekte svar siden sidste sværhedsgradsstigning
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

    public void ClearAnswerInput()
    {
        answerInput.text = ""; // Ryd inputfeltet
    }

    void IncreaseDifficulty()
    {
        difficultyLevel++;
        Debug.Log($"Difficulty increased to level {difficultyLevel}");

        GenerateProblem(); // Sørg for at generere en ny opgave efter at sværhedsgraden er blevet øget
        UpdateDifficultyLevelText(); // Opdater TMP text feltet for sværhedsgrad
    }


    void UpdateCorrectAnswersCountText()
    {
        correctAnswersCountText.text = $"Correct Answers: {correctAnswersCount}";
    }
    void UpdateDifficultyLevelText()
    {
        difficultyLevelText.text = $"Difficulty Level: {difficultyLevel}"; // Opdater TMP text feltet med sværhedsgraden
    }
}
