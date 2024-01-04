using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPult_RightButton : MonoBehaviour
{
    public bool ButtonDown = false;
    public Collider ComputerPult_RightButton_Collider;


    void Start()
    {
        ComputerPult_RightButton_Collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hand")
        {
            ButtonDown = true;
        }
    }
}
