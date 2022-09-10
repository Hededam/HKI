using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyButton : MonoBehaviour {
    Camera mainCam;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {

            print("Space pressed!");
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z + 10f);
            print(" position : " + mainCam.transform.position);
        }

    }
}
