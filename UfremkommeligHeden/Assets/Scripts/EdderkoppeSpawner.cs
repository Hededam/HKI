using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdderkoppeSpawner : MonoBehaviour
{
    public GameObject Edderkob;

    // Instantiate the prefab somewhere between -10.0 and 10.0 on the x-z plane 
    void Start()
    {
    }
    public void spawEdderkob()
    {
        Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
        Instantiate(Edderkob, position, Quaternion.identity);
    }
}