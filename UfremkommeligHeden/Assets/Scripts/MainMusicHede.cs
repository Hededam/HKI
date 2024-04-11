using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMusicHede : MonoBehaviour
{
    public AudioSource soundSource;
    public float masterSoundVolume;
    AudioSource _audioSource;

    public List<AudioClip> myClips = new List<AudioClip>();

    public Button myButton;
    public Button muteButton;
    public Button nextButton;
    public Button unmuteButton;
    public Button previousButton;

    private int currentClipIndex = 0;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        myButton.onClick.AddListener(() => PlayRandomSound(1.0f));
        muteButton.onClick.AddListener(Mute);
        unmuteButton.onClick.AddListener(Unmute);
        nextButton.onClick.AddListener(PlayNextSound);
        previousButton.onClick.AddListener(PlayPreviousSound);
    }

    public void PlayRandomSound(float vol)
    {
        int randomIndex = Random.Range(0, myClips.Count);
        AudioClip myClip = myClips[randomIndex];

        soundSource.clip = myClip;
        soundSource.volume = masterSoundVolume * vol;
        soundSource.Play();

        StartCoroutine(PlayNextClipAfterDelay(myClip.length)); // Tilføj denne linje
    }

    public void Mute()
    {
        soundSource.mute = true;
    }

    public void Unmute()
    {
        soundSource.mute = false;
    }

    public void PlayNextSound()
    {
        currentClipIndex = (currentClipIndex + 1) % myClips.Count;
        AudioClip myClip = myClips[currentClipIndex];

        soundSource.clip = myClip;
        soundSource.volume = masterSoundVolume;
        soundSource.Play();

        StartCoroutine(PlayNextClipAfterDelay(myClip.length)); // Tilføj denne linje
    }

    public void PlayPreviousSound()
    {
        currentClipIndex--;
        if (currentClipIndex < 0)
        {
            currentClipIndex = myClips.Count - 1;
        }
        AudioClip myClip = myClips[currentClipIndex];

        soundSource.clip = myClip;
        soundSource.volume = masterSoundVolume;
        soundSource.Play();

        StartCoroutine(PlayNextClipAfterDelay(myClip.length)); // Tilføj denne linje
    }

    // Tilføj denne coroutine for at afspille det næste klip efter en forsinkelse
    IEnumerator PlayNextClipAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayNextSound();
    }
}
