using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneJunp : MonoBehaviour
{
    // input scene nr her:
    public int JumpTo = 1;
    public bool senebool = false;
    // Når den coliderer, loader sene 
 

    void Update()
    {
        SceneManager.LoadScene(JumpTo); //bestemmer hvilken sene der skal loades
    }

 
}
