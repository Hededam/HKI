using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YellowAmogusDies : MonoBehaviour
{
    private AudioSource _audioSource;
    public float masterSoundVolume;
    public AudioSource soundSource;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (transform.position.y <= -20)
        {
            transform.position = new Vector3(1, 60, 45);
            _audioSource.Play();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Kill"))
        {
            Destroy(gameObject);
        }
    }
    public void PlaySound(float vol, AudioClip myClip)     //adjust preferred volume of particular clip in "vol" 
    {
        soundSource.clip = myClip;
        soundSource.volume = masterSoundVolume * vol;
        soundSource.Play();
    }
}

