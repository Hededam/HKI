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
        num1 = Random.Range(1, 10);
        num2 = Random.Range(1, 10);
        correctAnswer = num1 + num2;

        problemText.text = $"{num1} + {num2} = ?";
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
