using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampelScript : MonoBehaviour
{

    /*
     Assigment Operaters
     = + -
        
         
         
Logical operaters
&& And
 ||

         */



    public float speed = 0.0f;
    public float distence = 0.0f;
    public float time = 0.0f;




    // Start is called before the first frame update
    void Start()
    {


        speed = distence / time;
        print("du rejser " + speed + " kilomerter i timen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 }
