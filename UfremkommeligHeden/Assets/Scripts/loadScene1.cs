using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class loadScene1 : MonoBehaviour {
	// Naar den coliderer med table, load sene 1
public void OnCollisionEnter (Collision col)
    { if (col.gameObject.tag == "MagicCube") {
    SceneManager.LoadScene(2); //replace the int with level number
}
}
}