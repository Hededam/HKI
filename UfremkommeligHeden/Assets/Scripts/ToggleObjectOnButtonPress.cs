using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleObjectOnButtonPress : MonoBehaviour
{
    public GameObject objectToToggle; // Referencen til det GameObject, der skal aktiveres/deaktiveres
    private bool isActive = true; // Bool til at holde styr på om objektet er aktivt eller ej

    private void Start()
    {
        // Initial tilstand af objektet
        if (objectToToggle != null)
        {
            isActive = objectToToggle.activeSelf;
        }

        // Tilføj en listener til knappen, så den reagerer på klik
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (objectToToggle != null)
        {
            isActive = !isActive; // Skifter bool-værdien
            objectToToggle.SetActive(isActive); // Sætter objektet til enten aktiv eller inaktiv
        }
    }
}
