using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // start Here
using BNG;

public class LoadSceneOne : MonoBehaviour
{
    public Grabbable Handle;
    private GameObject rightGrabber;
    private GameObject leftGrabber;
    private Grabber rGrabber;
    private Grabber lGrabber;

    private void Start()
    {
        rightGrabber = GameObject.FindWithTag("RightGrabber");
        leftGrabber = GameObject.FindWithTag("LeftGrabber");
        rGrabber = rightGrabber.GetComponent<Grabber>();
        lGrabber = leftGrabber.GetComponent<Grabber>();
    }



    private void LetGo()
    {
        if (Handle.BeingHeld)
        {
            if (rGrabber.HeldGrabbable == Handle)
            {
                rGrabber.HeldGrabbable.DropItem(rGrabber);
            }
            else if (lGrabber.HeldGrabbable == Handle)
            {
                lGrabber.HeldGrabbable.DropItem(lGrabber);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            LetGo();
            SceneManager.LoadScene("SceneOne");
        }
    }
}
