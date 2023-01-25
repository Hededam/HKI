using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiveauKnap : MonoBehaviour
{
    public GameObject target;
    public GameObject Niveau1;
    public GameObject Niveau2;
    public GameObject Niveau3;
    public string noget;
    public void aktiverX()
    {

        target.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        Niveau1.SetActive(false);
        Niveau2.SetActive(false);
        Niveau3.SetActive(false);
    }
}
