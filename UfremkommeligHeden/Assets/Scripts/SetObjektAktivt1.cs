using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjektAktivt1 : MonoBehaviour
{
    public GameObject andetObjekt; // Referencen til det andet objekt

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Tjekker om objektet, der passerer, har tagget "Spiller"
        {
            SetObjektAktivt(); // Kalder metoden til at gøre det andet objekt aktivt
        }
    }

    private void SetObjektAktivt()
    {
        if (andetObjekt != null)
        {
            andetObjekt.SetActive(true);
            // Gør det andet objekt aktivt
        }
        else
        {
            Debug.LogWarning("Andet objekt er ikke tildelt i inspektøren.");
        }
    }
}
