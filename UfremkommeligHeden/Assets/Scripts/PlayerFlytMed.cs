using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlytMed : MonoBehaviour

{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Forbind Player med det bev�gelige objekt
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Frakobl Player fra det bev�gelige objekt
            collision.transform.parent = null;
        }
    }
}