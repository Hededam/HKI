using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Tilf�j denne linje for at bruge UI elementer

public class MainMusicHede : MonoBehaviour
{
    public AudioSource soundSource;
    public float masterSoundVolume;
    AudioSource _audioSource;

    public List<AudioClip> myClips = new List<AudioClip>();

    // Tilf�j denne linje for at referere til din knap
    public Button myButton;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Tilf�j denne linje for at tilknytte PlayRandomSound funktionen til knappens OnClick event
        myButton.onClick.AddListener(() => PlayRandomSound(1.0f)); // Du kan �ndre volumen parameteren som du vil
    }

    public void PlayRandomSound(float vol)     //adjust preferred volume of particular clip in "vol" 
    {
        int randomIndex = Random.Range(0, myClips.Count); // V�lg en tilf�ldig sang fra listen
        AudioClip myClip = myClips[randomIndex];

        soundSource.clip = myClip;
        soundSource.volume = masterSoundVolume * vol;
        soundSource.Play();
    }
}
