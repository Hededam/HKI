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
    public AudioClip wrongAnswerSound;
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
    private int operation;

    public bool isAdditionEnabled = true;
    public bool isSubtractionEnabled = true;
    public bool isMultiplicationEnabled = true;
    public bool isDivisionEnabled = true;
    public bool isSquareRootEnabled = true;
    public bool isWordProblemEnabled = true;

    public List<WordProblem> wordProblems = new List<WordProblem>
    {
        new WordProblem { Text = "Hvis en bil kører med en hastighed på 60 km/t, Hvor lang tid vil det tage at køre 240 km?", Answer = 4 },
        new WordProblem { Text = "Hvis en skole har 10 klasser, og hver klasse har 15 elever, hvor mange elever er der i alt på skolen?", Answer = 150 },
        new WordProblem { Text = "Hvis en landmand har 4 høns, og hver høne lægger 3 æg om dagen, hvor mange æg får landmanden om dagen?", Answer = 12 },
        new WordProblem { Text = "Hvis en bog har 200 sider, og du læser 25 sider om dagen, hvor mange dage vil det tage at læse hele bogen?", Answer = 8 },
        new WordProblem { Text = "Hvis en cykel koster 3000 kr, og du sparer 200 kr om måneden, hvor mange måneder vil det tage at spare op til cyklen?", Answer = 15 },
        new WordProblem { Text = "Hvis en bager har 25 brød, og hver kunde køber 2 brød, hvor mange kunder kan bageren betjene?", Answer = 12 },
        new WordProblem { Text = "Hvis der er 12 fugle på et træ, 7 flyver væk, og 5 mere lander på træet, hvor mange fugle er der nu?", Answer = 10 },
        new WordProblem { Text = "Hvis Anna har 15 blyanter, hun giver 5 til sin ven og køber 10 mere, hvor mange blyanter har hun nu?", Answer = 20 },
        new WordProblem { Text = "Hvis Bob har ti æbler, efter han har givet to til Mads og selv spist tre, hvor mange har han så tilbage?", Answer = 5 },
        new WordProblem { Text = "Hvis en bil kører med en hastighed på 70 km/t, hvor lang tid vil det tage at køre 280 km?", Answer = 4 },
        new WordProblem { Text = "Hvis en skole har 9 klasser, og hver klasse har 20 elever, hvor mange elever er der i alt på skolen?", Answer = 180 },
        new WordProblem { Text = "Hvis en landmand har 5 høns, og hver høne lægger 4 æg om dagen, hvor mange æg får landmanden om dagen?", Answer = 20 },
        new WordProblem { Text = "Hvis en bog har 250 sider, og du læser 50 sider om dagen, hvor mange dage vil det tage at læse hele bogen?", Answer = 5 },
        new WordProblem { Text = "Hvis en cykel koster 3500 kr, og du sparer 700 kr om måneden, hvor mange måneder vil det tage at spare op til cyklen?", Answer = 5 },
        new WordProblem { Text = "Hvis en bager har 30 brød, og hver kunde køber 3 brød, hvor mange kunder kan bageren betjene?", Answer = 10 },
        new WordProblem { Text = "Hvis der er 15 fugle på et træ, 5 flyver væk, og 7 mere lander på træet, hvor mange fugle er der nu?", Answer = 17 },
        new WordProblem { Text = "Hvis Anna har 20 blyanter, hun giver 5 til sin ven og køber 15 mere, hvor mange blyanter har hun nu?", Answer = 30 },
        new WordProblem { Text = "Hvis Bob har 15 æbler, efter han har givet 3 til Mads og selv spist 4, hvor mange har han så tilbage?", Answer = 8 },
        new WordProblem { Text = "Hvis en bil kører med en hastighed på 80 km/t, hvor lang tid vil det tage at køre 320 km?", Answer = 4 },
        new WordProblem { Text = "Hvis en skole har 8 klasser, og hver klasse har 25 elever, hvor mange elever er der i alt på skolen?", Answer = 200 },
        new WordProblem { Text = "Hvis en landmand har 6 høns, og hver høne lægger 5 æg om dagen, hvor mange æg får landmanden om dagen?", Answer = 30 },
        new WordProblem { Text = "Hvis en bog har 300 sider, og du læser 60 sider om dagen, hvor mange dage vil det tage at læse hele bogen?", Answer = 5 },
        new WordProblem { Text = "Hvis en cykel koster 4000 kr, og du sparer 800 kr om måneden, hvor mange måneder vil det tage at spare op til cyklen?", Answer = 5 },
        new WordProblem { Text = "Hvis en bager har 35 brød, og hver kunde køber 7 brød, hvor mange kunder kan bageren betjene?", Answer = 5 },
        new WordProblem { Text = "Hvis der er 18 fugle på et træ, 6 flyver væk, og 9 mere lander på træet, hvor mange fugle er der nu?", Answer = 21 },
        new WordProblem { Text = "Hvis Anna har 25 blyanter, hun giver 5 til sin ven og køber 20 mere, hvor mange blyanter har hun nu?", Answer = 40 },
        new WordProblem { Text = "Hvis Bob har 20 æbler, efter han har givet 4 til Mads og selv spist 5, hvor mange har han så tilbage?", Answer = 11 },
        new WordProblem { Text = "Hvis en bil kører med en hastighed på 90 km/t, hvor lang tid vil det tage at køre 360 km?", Answer = 4 },
        new WordProblem { Text = "Hvis en skole har 7 klasser, og hver klasse har 30 elever, hvor mange elever er der i alt på skolen?", Answer = 210 },
        new WordProblem { Text = "Hvis en landmand har 7 høns, og hver høne lægger 6 æg om dagen, hvor mange æg får landmanden om dagen?", Answer = 42 },
        new WordProblem { Text = "Hvis en bog har 350 sider, og du læser 70 sider om dagen, hvor mange dage vil det tage at læse hele bogen?", Answer = 5 },
        new WordProblem { Text = "Hvis en cykel koster 4500 kr, og du sparer 900 kr om måneden, hvor mange måneder vil det tage at spare op til cyklen?", Answer = 5 },
        new WordProblem { Text = "Hvis en bager har 40 brød, og hver kunde køber 8 brød, hvor mange kunder kan bageren betjene?", Answer = 5 },
        new WordProblem { Text = "Hvis der er 21 fugle på et træ, 7 flyver væk, og 11 mere lander på træet, hvor mange fugle er der nu?", Answer = 25 },
        new WordProblem { Text = "Hvis Anna har 30 blyanter, hun giver 6 til sin ven og køber 24 mere, hvor mange blyanter har hun nu?", Answer = 48 },
        new WordProblem { Text = "Hvis Bob har 25 æbler, efter han har givet 5 til Mads og selv spist 6, hvor mange har han så tilbage?", Answer = 14 },
        new WordProblem { Text = "Hvis en bil kører med en hastighed på 100 km/t, hvor lang tid vil det tage at køre 400 km?", Answer = 4 }

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
        operation = Random.Range(0, 5);
        // Tjek om alle operationer er deaktiveret
        if (!isAdditionEnabled && !isSubtractionEnabled && !isMultiplicationEnabled && !isDivisionEnabled && !isSquareRootEnabled && !isWordProblemEnabled)
        {
            problemText.text = "Ingen opgave typer er aktiverede. Er man lidt doven? Aktivér mindst én type opgave for at fortsætte.";
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
                    num1 = Random.Range(60, 70);
                    num2 = Random.Range(60, 70);
                    break;
                case 7:
                    num1 = Random.Range(70, 80);
                    num2 = Random.Range(70, 80);
                    break;
                case 8:
                    num1 = Random.Range(80, 90);
                    num2 = Random.Range(80, 90);
                    break;
                case 9:
                    num1 = Random.Range(90, 100);
                    num2 = Random.Range(90, 100);
                    break;
                case 10:
                    num1 = Random.Range(1, 10000);
                    num2 = Random.Range(1, 10000);
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

                            GameObject Gamestuff = GameObject.FindWithTag("Gamestuff");

                if (Gamestuff != null)
                {
                    // Tjek om objektet har PlayerXp scriptet tilknyttet
                    PlayerXp playerXp = Gamestuff.GetComponent<PlayerXp>();

                    if (playerXp != null)
                    {
                        // Bestem belønningen baseret på operationstypen
                       switch (operation) 
                        {
                            case 0: // Addition
                                playerXp.GainXP(10); // Tilføj 10 XP
                                playerXp.PlayTimeLeft += 30; // Tilføj 30 sekunder til spilletiden
                                break;
                            case 1: // Subtraction
                                playerXp.GainXP(50); // Tilføj 50 XP
                                playerXp.PlayTimeLeft += 30; // Tilføj 30 sekunder til spilletiden
                                break;
                            case 2: // Multiplication
                                playerXp.GainXP(75); // Tilføj 75 XP
                                playerXp.PlayTimeLeft += 45; // Tilføj 45 sekunder til spilletiden
                                break;
                            case 3: // Division
                                playerXp.GainXP(100); // Tilføj 100 XP
                                playerXp.PlayTimeLeft += 60; // Tilføj 60 sekunder til spilletiden
                                break;
                            case 4: // Square root
                                playerXp.GainXP(125); // Tilføj 125 XP
                                playerXp.PlayTimeLeft += 75; // Tilføj 75 sekunder til spilletiden
                                break;
                            default:
                                Debug.LogError("Invalid operation!");
                                break;
                        }
                    }
                    else
                    {
                        Debug.Log("PlayerXp script not found on player object.");
                    }
                }
                else
                {
                    Debug.Log("Player object not found.");
                }

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
          
             if (wrongAnswerSound != null)
                {
                    AudioSource.PlayClipAtPoint(wrongAnswerSound, transform.position);
                }

                GenerateProblem();
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
