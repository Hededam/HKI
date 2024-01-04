using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPult_LeftButton : MonoBehaviour
{
    public bool ButtonDown = false;
    public Collider ComputerPult_LeftButton_Collider;




    // Start is called before the first frame update
    void Start()
    {
        ComputerPult_LeftButton_Collider = GetComponent<Collider>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hand")
        {
            ButtonDown = true;
        }
    }

   /* private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Hand")
        {
            ButtonDown = false;
        }
    }*/
}
