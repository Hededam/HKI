using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiveauKnap : MonoBehaviour
{
    public GameObject Niveau1;
    public GameObject Niveau2;
    public GameObject Niveau3;
    public void aktiverX()
    {
      
        Niveau1.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        Niveau1.SetActive(false);
        Niveau2.SetActive(false);
        Niveau3.SetActive(false);
    }
}
