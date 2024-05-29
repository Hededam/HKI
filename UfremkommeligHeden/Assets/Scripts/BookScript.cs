using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookScript : MonoBehaviour
{
    public List<Material> materials; // Din liste af materialer
    public GameObject bookPage1; // Dit første GameObject
    public GameObject bookPage2; // Dit andet GameObject
    private int currentMaterialIndex = 0;

    // Metode til at skifte materialer fremad
    public void ChangeMaterialForward()
    {
        // Tjekker om listen af materialer ikke er tom
        if (materials.Count == 0)
        {
            Debug.Log("Ingen materialer i listen");
            return;
        }

        // Skifter materialet på de to GameObjects
        bookPage1.GetComponent<Renderer>().material = materials[currentMaterialIndex];
        bookPage2.GetComponent<Renderer>().material = materials[currentMaterialIndex];

        // Opdaterer indekset for det nuværende materiale
        currentMaterialIndex = (currentMaterialIndex + 1) % materials.Count;
    }

    // Metode til at skifte materialer baglens
    public void ChangeMaterialBackward()
    {
        // Tjekker om listen af materialer ikke er tom
        if (materials.Count == 0)
        {
            Debug.Log("Ingen materialer i listen");
            return;
        }

        // Opdaterer indekset for det nuværende materiale
        currentMaterialIndex = ((currentMaterialIndex - 1) + materials.Count) % materials.Count;

        // Skifter materialet på de to GameObjects
        bookPage1.GetComponent<Renderer>().material = materials[currentMaterialIndex];
        bookPage2.GetComponent<Renderer>().material = materials[currentMaterialIndex];
    }
}
