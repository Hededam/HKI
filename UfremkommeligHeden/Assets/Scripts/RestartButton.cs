using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("Jumping-scene");
    }
}
