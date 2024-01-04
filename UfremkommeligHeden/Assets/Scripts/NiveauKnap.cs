using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiveauKnap : MonoBehaviour
{
    public GameObject target;
    public GameObject Niveau1;
    public GameObject Niveau2;
    public GameObject Niveau3;
    public GameObject Niveau4;
    public GameObject Niveau5;
    public GameObject Niveau6;
    public string noget;
    public void aktiverDown()
    {
        Niveau1.SetActive(false);
        Niveau2.SetActive(false);
        Niveau3.SetActive(false);
        Niveau4.SetActive(false);
        Niveau5.SetActive(false);
        Niveau6.SetActive(false);
    }
    public void aktiverUP()
    {
 
        target.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        Niveau1.SetActive(false);
        Niveau2.SetActive(false);
        Niveau3.SetActive(false);
        Niveau4.SetActive(false);
        Niveau5.SetActive(false);
        Niveau6.SetActive(false);
    }
}
