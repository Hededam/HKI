using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour
{
    public AudioSource soundSource;
    public float masterSoundVolume; 
    AudioSource _audioSource;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(float vol, AudioClip myClip)     //adjust preferred volume of particular clip in "vol" 
    {
        soundSource.clip = myClip;
        soundSource.volume = masterSoundVolume * vol;
        soundSource.Play();
    }
}
