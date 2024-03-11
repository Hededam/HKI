using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerOnMovingPlatforms : MonoBehaviour
{
    private GameObject theChild;
   // private Vector3 Playerscale;

    private void Start()
    {
      //  Playerscale = new Vector3(1f,1f,1f);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            theChild = collision.gameObject;
            theChild.transform.SetParent(this.transform, true);
          //  theChild.transform.localScale.x = 1;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            theChild.transform.SetParent(null);
        }
    }
}