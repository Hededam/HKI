using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RingBase : MonoBehaviour
{
    private RingCourse course;

    public int ringID;

    // Start is called before the first frame update
    void awake()
    {
        course = GetComponentInParent<RingCourse>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ring entered");
            course.RingPassed(ringID);
        }
    }
}
