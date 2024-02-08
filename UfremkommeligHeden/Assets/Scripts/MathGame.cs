using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MathGame : MonoBehaviour
{
    public TextMeshProUGUI problemText;
    public TMP_InputField answerInput;
    public AudioClip correctAnswerSound;
    public GameObject newGameObjectPrefab;


    public TextMeshProUGUI[] numberButtons;
    public Button checkAnswerButton;

    private int num1;
    private int num2;
    private int correctAnswer;

    void Start()
    {
        GenerateProblem();

        // Tilføj OnClick-events til knapperne
        for (int i = 0; i < numberButtons.Length; i++)
        {
            int buttonValue = i + 1;
            numberButtons[i].text = buttonValue.ToString();
            int index = i; // Gem variabel i lokalt omfang for at undgå lukninger
            numberButtons[i].gameObject.GetComponent<Button>().onClick.AddListener(() => OnNumberButtonClick(buttonValue));
        }

        // Tilføj OnClick-event til tjek svaret-knappen
        checkAnswerButton.onClick.AddListener(CheckAnswer);
    }

    void GenerateProblem()
    {
        int operation = Random.Range(0, 4); // 0: Addition, 1: Subtraktion, 2: Multiplikation, 3: Division
        switch (operation)
        {
            case 0: // Addition
                num1 = Random.Range(1, 20);
                num2 = Random.Range(1, 20);
                correctAnswer = num1 + num2;
                problemText.text = $"{num1} + {num2} = ?";
                break;
            case 1: // Subtraktion
                num1 = Random.Range(10, 20);
                num2 = Random.Range(1, 10);
                correctAnswer = num1 - num2;
                problemText.text = $"{num1} - {num2} = ?";
                break;
            case 2: // Multiplikation
                num1 = Random.Range(2, 100);
                num2 = Random.Range(2, 100);
                correctAnswer = num1 * num2;
                problemText.text = $"{num1} * {num2} = ?";
                break;
            case 3: // Division
                correctAnswer = Random.Range(2, 10); // Tallet, vi skal dividere med
                num2 = Random.Range(2, 10); // Divisoren
                num1 = correctAnswer * num2; // Resultatet
                problemText.text = $"{num1} ÷ {num2} = ?";
                break;
            default:
                Debug.LogError("Invalid operation!");
                break;
        }
    }

    void OnNumberButtonClick(int buttonValue)
    {
        answerInput.text += buttonValue.ToString();
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
