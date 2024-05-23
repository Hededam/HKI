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
new WordProblem { Text = "Her er en svær, Hvis en rumraket flyver med en hastighed på 28.000 km/t, hvor lang tid vil det tage at nå Månen, der er 384.400 km væk?", Answer = 14 },
new WordProblem { Text = "En troldmand fremtryller 7 kaniner fra sin hat hver dag, bare fordi han ikke har andet at lave, hvor mange kaniner vil han have efter en uge, hvis vi går ud fra at de ikke pare sig?", Answer = 49 },
new WordProblem { Text = "En IT-underviser kan løfte 100 gange sin egen vægt, og han vejer 70 kg, hvor menge Kg. kan han løfte?", Answer = 7000 },
new WordProblem { Text = "Hvis en robot kan bygge en bil på 6 timer, hvor mange biler kan den bygge på en dag?", Answer = 4 },
new WordProblem { Text = "Hvis en pirat finder en skattekiste med 500 guldmønter og deler dem ligeligt mellem sig selv og sine 4 besætningsmedlemmer, hvor mange mønter får hver person?", Answer = 100 },
new WordProblem { Text = "Hvis en prinsesse har 20 par sko men mister 7 par på en rejse, og så køber 15 par mere, Hvor mange par sko har hun så?", Answer = 28 },
new WordProblem { Text = "Her er en lidt svær, Hvis en astronaut tager 3 skridt på Månen, og hvert skridt er 1,5 gange længere end på Jorden, hvor mange 'jord-skridt' har astronauten så taget?", Answer = 5 },
new WordProblem { Text = "Hvis en detektiv løser 2 mysterier om dagen, hvor mange mysterier vil detektiven have løst på en uge?", Answer = 14 },
new WordProblem { Text = "Hvis en ninja kaster 5 stjerner hvert minut, hvor mange stjerner vil ninjaen have kastet på en time?", Answer = 300 },
new WordProblem { Text = "Hvis en zombie spiser 3 hjerner om dagen, hvor mange hjerner vil zombien have spist på en uge?", Answer = 21 },
new WordProblem { Text = "Hvis en vampyr drikker 2 liter blod hver nat, hvor meget blod vil vampyren have drukket på en måned (30 dage)?", Answer = 60 },
new WordProblem { Text = "Hvis et rumvæsen har 5 øjne og møder 4 andre rumvæsener, hvor mange øjne ser rumvæsenet i alt?", Answer = 20 },
new WordProblem { Text = "Hvis en heks brygger 2 trylledrikke om dagen, og hver trylledrik kræver 3 edderkopper, hvor mange edderkopper vil heksen bruge på en uge?", Answer = 42 },
new WordProblem { Text = "Hvis en superhelt redder 4 grimme børn fra brandende bygninger hver dag, hvor mange børn vil superhelten have reddet på en måned (30 dage)?", Answer = 120 },
new WordProblem { Text = "Hvis en robot kan danse i 15 minutter på et enkelt batteri, hvor mange timer kan robotten danse med 12 batterier?", Answer = 3 },
new WordProblem { Text = "Hvis en enhjørning løber 5 km om dagen, hvor mange km vil enhjørningen have løbet på en måned (30 dage)?", Answer = 150 },
new WordProblem { Text = "Hvis en drage samler 3 skatte om dagen, hvor mange skatte vil dragen have samlet på en uge?", Answer = 21 },
new WordProblem { Text = "Hvis en snegl bevæger sig 0,03 km/t, hvor mange dage vil det tage for sneglen at bevæge sig 1 km?", Answer = 33 },
new WordProblem { Text = "Hvis en myre kan bære 50 gange sin egen vægt, og den vejer 3 mg, hvor meget kan myren bære?", Answer = 150 },
new WordProblem { Text = "Hvis en frø kan hoppe 20 gange sin egen længde, og den er 5 cm lang, hvor langt kan frøen hoppe?", Answer = 100 },
new WordProblem { Text = "Hvis en kakerlak kan lægge 5 æg om dagen, hvor mange æg vil kakerlakken have lagt på en måned (30 dage)?", Answer = 150 },
new WordProblem { Text = "Hvis en flue kan producere 10 gram afføring om dagen, hvor meget afføring vil fluen have produceret på en uge? (og ja det er en temmelig stor flue)", Answer = 70 },
new WordProblem { Text = "Hvis en rotte kan gnave gennem 2 cm træ om dagen, hvor lang tid vil det tage for rotten at gnave gennem en 30 cm tyk trædør?", Answer = 15 },
new WordProblem { Text = "Hvis en skorpion stikker 3 gange om dagen, hver dag i en uge, hvor mange gange vil skorpionen have stukket på den uge?", Answer = 21 },
new WordProblem { Text = "Hvis en edderkop spinder 4 meter spindelvæv om dagen, hvor mange meter spindelvæv vil edderkoppen have spundet på en måned (30 dage)?", Answer = 120 },
new WordProblem { Text = "Hvis en drage ånder ild og brænder 4 slotte ned hver måned, hvor mange slotte vil ligge i ruiner efter et år?", Answer = 48 },
new WordProblem { Text = "En zombiehær angriber en by med 1000 indbyggere. Hver zombie kan æde 5 menneskehjerner om dagen. Hvor mange dage tager det, før byen er hjerneløs", Answer = 200 },
new WordProblem { Text = "Hvis en IT lære Mikkel kan spise 10 kilo slik på en time, hvor meget slik vil han have spist efter 3 timer, hvis han ikke får ondt i maven?", Answer = 30 },
new WordProblem { Text = "Hvis en varulv æder 4 dumme børn hver fuldmåne, hvor mange dumme unger vil han have spist efter 3 fuldmåner?", Answer = 12 },


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

        // Hvis kun wordProblems er aktiveret, generer et nyt wordProblem
        if (isWordProblemEnabled && !isAdditionEnabled && !isSubtractionEnabled && !isMultiplicationEnabled && !isDivisionEnabled && !isSquareRootEnabled)
        {
            if (wordProblems.Count > 0)
            {
                int index = Random.Range(0, wordProblems.Count);
                var problem = wordProblems[index];
                problemText.text = problem.Text;
                correctAnswer = problem.Answer;
                wordProblems.RemoveAt(index); // Fjern det valgte problem fra listen
            }
            else
            {
                problemText.text = "Der er ikke flere wordProblems tilbage. Aktivér en anden type opgave for at fortsætte.";
                return;
            }
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
                    Debug.LogError("fejl i GenerateProblem()");
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
                    PlayerXp playerXp = Gamestuff.GetComponent<PlayerXp>();
                    if (playerXp != null)
                    {
                        // Bestem belønningen baseret på operationstypen
                       switch (operation) 
                        {
                            case 0: // Addition
                                playerXp.GainXP(10); // Tilføj 50 XP
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
                                Debug.LogError("fejl i CheckAnswer()");
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
        correctAnswersCountText.text = $"Rigtige svar: {correctAnswersCount}";
    }

    void UpdateWrongAnswersCountText()
    {
        wrongAnswersCountText.text = $"Forkerte svar: {wrongAnswersCount}";
    }

    void UpdateDifficultyLevelText()
    {
        difficultyLevelText.text = $"Sværhedsgrad: {difficultyLevel}";
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
