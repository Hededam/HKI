using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonToInput : MonoBehaviour
{
    public TMP_InputField inputField;

    // Metode til at indstille tekstfeltet baseret på knappens tekst
    public void SetInputFieldText(string text)
    {
        inputField.text += text;
    }
}

