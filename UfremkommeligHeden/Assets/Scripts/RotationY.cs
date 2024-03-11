using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotationy : MonoBehaviour
{
    public int _rotationSpeed = 30;
    
    void Update()
    {
     transform.Rotate(_rotationSpeed * Time.deltaTime, 0, 0);
    }
}
