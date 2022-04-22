using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureStuff : MonoBehaviour
{
    public GameObject fractured;
    private Collider Col;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            Instantiate(fractured, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
