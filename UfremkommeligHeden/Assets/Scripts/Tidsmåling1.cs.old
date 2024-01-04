using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tidsmåling : MonoBehaviour
{
    public Text tidTekst;

    private float startTime;
    private bool raceStarted = false;
    private bool raceFinished = false;

    void Start()
    {
        tidTekst.text = "Tid: 0.00 sekunder";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartRing"))
        {
            if (!raceStarted)
            {
                raceStarted = true;
                startTime = Time.time;
                Debug.Log("Løbet er startet!");
            }
        }

        if (other.CompareTag("MålRing") && raceStarted && !raceFinished)
        {
            float endTime = Time.time - startTime;
            string formattedTime = string.Format("{0:0.00}", endTime);
            tidTekst.text = "Tid: " + formattedTime + " sekunder";
            raceFinished = true;
        }
    }

    void Update()
    {
        if (raceStarted && !raceFinished)
        {
            float currentTime = Time.time - startTime;
            string formattedTime = string.Format("{0:0.00}", currentTime);
            tidTekst.text = "Tid: " + formattedTime + " sekunder";
        }
    }
}