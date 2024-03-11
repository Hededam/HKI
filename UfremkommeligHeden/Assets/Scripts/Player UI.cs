using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour{
public Text myText;

private int test = 0;

    void Update()
    {
        myText.text = test.ToString("Get to the goal!");

        test++;
    }
}
