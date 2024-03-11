using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterKillSound : MonoBehaviour
{
    public AudioSource soundSource;
    public float masterSoundVolume; 
    private AudioSource _audioSource;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sus"))
        {
            _audioSource.Play();
        }
    }
    public void PlaySound(float vol, AudioClip myClip)     //adjust preferred volume of particular clip in "vol" 
    {
        soundSource.clip = myClip;
        soundSource.volume = masterSoundVolume * vol;
        soundSource.Play();
    }
}
