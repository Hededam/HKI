using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WordProblem
{
    public string Text { get; set; }
    public int Answer { get; set; }
}

public class MathGame : MonoBehaviour
{
    public TextMeshProUGUI problemText;
    public TMP_InputField answerInput;
    public AudioClip correctAnswerSound;
    public GameObject newGameObjectPrefab;
    public TextMeshProUGUI correctAnswersCountText;
    public TextMeshProUGUI wrongAnswersCountText;
    public TextMeshProUGUI difficultyLevelText;
    public int correctAnswersCount { get; private set; }
    public int wrongAnswersCount { get; private set; }
    public int correctAnswersBeforeIncrease = 3;
    public int difficultyLevel = 0;
    private int num1;
    private int num2;
    private int correctAnswer;
    private int wrongAnswer;
    private int correctAnswersSinceIncrease;
    private AudioSource audioSource;

    public bool isAdditionEnabled = true;
    public bool isSubtractionEnabled = true;
    public bool isMultiplicationEnabled = true;
    public bool isDivisionEnabled = true;
    public bool isSquareRootEnabled = true;
    public bool isWordProblemEnabled = true;

    public List<WordProblem> wordProblems = new List<WordProblem>
    {
        new WordProblem { Text = "Hvis en bil kører med en hastighed på 60 km/t, " +
            "Hvor lang tid vil det tage at køre 240 km?", Answer = 4 },
       
    };

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GenerateProblem();
        UpdateCorrectAnswersCountText();
        UpdateDifficultyLevelText();
    }

    void GenerateProblem()
    {
        // Tjek om alle operationer er deaktiveret
        if (!isAdditionEnabled && !isSubtractionEnabled && !isMultiplicationEnabled && !isDivisionEnabled && !isSquareRootEnabled && !isWordProblemEnabled)
        {
            problemText.text = "Ingen opgave typer er aktiverede. Er man lidt doven? Aktivér mindst én operation for at fortsætte.";
            return;
        }

        if (isWordProblemEnabled && Random.Range(0, 2) == 0) // Tilfældigt vælg mellem en WordProblem og en normal matematisk operation
        {
            var problem = wordProblems[Random.Range(0, wordProblems.Count)];
            problemText.text = problem.Text;
            correctAnswer = problem.Answer;
        }
        else
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
                default:
                    num1 = Random.Range(1, 10);
                    num2 = Random.Range(1, 10);
                    break;
            }

            int operation = Random.Range(0, 5);
            while ((operation == 0 && !isAdditionEnabled) ||
                   (operation == 1 && !isSubtractionEnabled) ||
                   (operation == 2 && !isMultiplicationEnabled) ||
                   (operation == 3 && !isDivisionEnabled) ||
                   (operation == 4 && !isSquareRootEnabled))
            {
                operation = Random.Range(0, 5);
            }

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
                case 4:
                    num1 = Random.Range(2, 10);
                    correctAnswer = num1 * num1;
                    problemText.text = $"√{correctAnswer} = ?";
                    break;
                default:
                    Debug.LogError("Invalid operation!");
                    break;
            }
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
                correctAnswersSinceIncrease++;
                UpdateCorrectAnswersCountText();

                if (correctAnswersSinceIncrease >= correctAnswersBeforeIncrease)
                {
                    IncreaseDifficulty();
                    correctAnswersSinceIncrease = 0;
                }

                GenerateProblem();
            }
            else
            {
                wrongAnswersCount++;
                UpdateWrongAnswersCountText();
                Debug.Log("Wrong answer. Try again!");
            }

            answerInput.text = "";
        }
    }

    public void ClearAnswerInput()
    {
        answerInput.text = "";
    }

    void IncreaseDifficulty()
    {
        difficultyLevel++;
        Debug.Log($"Difficulty increased to level {difficultyLevel}");

        GenerateProblem();
        UpdateDifficultyLevelText();
    }

    void UpdateCorrectAnswersCountText()
    {
        correctAnswersCountText.text = $"Correct Answers: {correctAnswersCount}";
    }

    void UpdateWrongAnswersCountText()
    {
        wrongAnswersCountText.text = $"Wrong Answers: {wrongAnswersCount}";
    }

    void UpdateDifficultyLevelText()
    {
        difficultyLevelText.text = $"Difficulty Level: {difficultyLevel}";
    }

    public void ToggleAddition()
    {
        isAdditionEnabled = !isAdditionEnabled;
    }

    public void ToggleSubtraction()
    {
        isSubtractionEnabled = !isSubtractionEnabled;
    }

    public void ToggleMultiplication()
    {
        isMultiplicationEnabled = !isMultiplicationEnabled;
    }

    public void ToggleDivision()
    {
        isDivisionEnabled = !isDivisionEnabled;
    }

    public void ToggleSquareRoot()
    {
        isSquareRootEnabled = !isSquareRootEnabled;
    }
    public void ToggleWordProblem()
    {
        isWordProblemEnabled = !isWordProblemEnabled;
    }
}
