using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleObjectOnButtonPress : MonoBehaviour
{
    public GameObject objectToToggle; // Referencen til det GameObject, der skal aktiveres/deaktiveres
    private bool isActive = true; // Bool til at holde styr p� om objektet er aktivt eller ej

    private void Start()
    {
        // Initial tilstand af objektet
        if (objectToToggle != null)
        {
            isActive = objectToToggle.activeSelf;
        }

        // Tilf�j en listener til knappen, s� den reagerer p� klik
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (objectToToggle != null)
        {
            isActive = !isActive; // Skifter bool-v�rdien
            objectToToggle.SetActive(isActive); // S�tter objektet til enten aktiv eller inaktiv
        }
    }
}
