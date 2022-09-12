using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPult_DepatureButton : MonoBehaviour
{
    public bool ButtonDown = false;
    public Collider ComputerPult_DepatureButton_Collider;


    void Start()
    {
        ComputerPult_DepatureButton_Collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hand")
        {
            ButtonDown = true;
            print("hit");
        }
    }
}