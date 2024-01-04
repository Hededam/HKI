using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class loadScene : MonoBehaviour
{
    // Naar den coliderer med table, load sene 1
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "MagicCube")
        {
            SceneManager.LoadScene(1); //replace the int with level number
        }
    }
}