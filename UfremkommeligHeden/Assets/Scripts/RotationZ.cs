using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotationz : MonoBehaviour
{
    public int _rotationSpeed = 30;
    
    void Update()
    {
     transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
}
