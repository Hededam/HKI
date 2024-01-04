using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consol_botten : MonoBehaviour
{
    public GameObject body;
    private bool visible = false;


    void onTriggerEnter(Collider other)
    {
        if (visible == false)
        {
            body.SetActive(true);
            visible = true;
        }
        
        else
        {
            body.SetActive(false);
            visible = false;
        }
    }
}
