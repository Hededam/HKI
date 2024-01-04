using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystik_input : MonoBehaviour
{
    public GameObject cube;



void onTriggerEnter(Collider other)
{
        cube.SetActive(false);
        
}
}
