using UnityEngine;
using TMPro;

public class MathGame : MonoBehaviour
{
    public TextMeshProUGUI problemText;
    public TMP_InputField answerInput;
    public AudioClip correctAnswerSound;
    public AudioClip increaseDifficultySound;
    public GameObject newGameObjectPrefab;
    public TextMeshProUGUI correctAnswersCountText;

    public int correctAnswersCount { get; private set; }
    private int num1;
    private int num2;
    private int correctAnswer;
    private int difficultyLevel;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GenerateProblem();
        UpdateCorrectAnswersCountText();
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
                // Swap num1 and num2 for subtraction operation
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
                UpdateCorrectAnswersCountText();
                if (correctAnswersCount >= 3)
                {
                    IncreaseDifficulty();
                    correctAnswersCount = 0;
                }
                else
                {
                    GenerateProblem(); // Flyt opgavegenereringslogikken her
                }
            }
            else
            {
                Debug.Log("Wrong answer. Try again!");
            }

            answerInput.text = "";
        }
    }

    void IncreaseDifficulty()
    {
        difficultyLevel++;
        Debug.Log($"Difficulty increased to level {difficultyLevel}");

        // Tjek om lydeffekten er tildelt korrekt
        if (increaseDifficultySound != null)
        {
            Debug.Log("Attempting to play increaseDifficultySound...");
            if (audioSource == null)
            {
                Debug.LogError("AudioSource is not assigned!");
            }
            else
            {
                audioSource.PlayOneShot(increaseDifficultySound);
                Debug.Log("increaseDifficultySound played successfully!");
            }
        }
        else
        {
            Debug.LogWarning("increaseDifficultySound is not assigned!");
        }
    }

    void UpdateCorrectAnswersCountText()
    {
        correctAnswersCountText.text = $"Correct Answers: {correctAnswersCount}";
    }
}
