using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Tilføj denne linje for at bruge UI elementer

public class MainMusicHede : MonoBehaviour
{
    public AudioSource soundSource;
    public float masterSoundVolume;
    AudioSource _audioSource;

    public List<AudioClip> myClips = new List<AudioClip>();

    // Tilføj denne linje for at referere til din knap
    public Button myButton;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Tilføj denne linje for at tilknytte PlayRandomSound funktionen til knappens OnClick event
        myButton.onClick.AddListener(() => PlayRandomSound(1.0f)); // Du kan ændre volumen parameteren som du vil
    }

    public void PlayRandomSound(float vol)     //adjust preferred volume of particular clip in "vol" 
    {
        int randomIndex = Random.Range(0, myClips.Count); // Vælg en tilfældig sang fra listen
        AudioClip myClip = myClips[randomIndex];

        soundSource.clip = myClip;
        soundSource.volume = masterSoundVolume * vol;
        soundSource.Play();
    }
}
