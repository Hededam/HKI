using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCourse : MonoBehaviour
{
    [SerializeField] private GameObject[] rings;
    private float time = 0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    internal void RingPassed(int ringNumber)
    {
        Debug.Log("ring " + ringNumber + " passed");
    }
}
