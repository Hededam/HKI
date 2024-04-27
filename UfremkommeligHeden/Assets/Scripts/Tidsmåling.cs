using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tidsmåling : MonoBehaviour
{
    public Text tidTekst;
    public AudioClip finishSound;
    private AudioSource audioSource;
    public ParticleSystem finishParticles; // Tilføj dette for at vælge dit partikelsystem

    private float startTime;
    private bool raceStarted = false;
    private bool raceFinished = false;

    void Start()
    {
        tidTekst.text = "Tid: 0.00 sekunder";
        audioSource = GetComponent<AudioSource>();

        // Slå partikelsystemet fra ved start, da det først skal aktiveres når målringen passeres
        if (finishParticles != null)
        {
            finishParticles.Stop();
        }
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
            // XP til spiller           
            GameObject playerObject = GameObject.FindWithTag("Player");

                if (playerObject != null)
                {
                    // Tjek om objektet har PlayerXp scriptet tilknyttet
                    PlayerXp playerXp = playerObject.GetComponent<PlayerXp>();

                    if (playerXp != null)
                    {  playerXp.GainXP(1000); // Tilføj 1000 XP
                    }
                }
            
            raceFinished = true;

            // Afspil lyden når målringen er passeret
            if (finishSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(finishSound);
            }

            // Aktiver partikelsystemet når målringen er passeret
            if (finishParticles != null)
            {
                finishParticles.Play();
            }
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
