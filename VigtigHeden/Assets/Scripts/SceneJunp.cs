using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneJunp : MonoBehaviour
{
    // input scene nr her:
    public int JumpTo = 1; 

    // Når den coliderer, loader sene 
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Potal")
        {
            SceneManager.LoadScene(JumpTo); //bestemmer hvilken sene der skal loades
        }
    }
}
