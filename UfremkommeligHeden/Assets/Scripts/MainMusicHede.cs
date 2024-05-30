using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMusicHede : MonoBehaviour
{
    public AudioSource soundSource;
    public float masterSoundVolume;
    AudioSource _audioSource;

    public List<AudioClip> myClips = new List<AudioClip>();
    public List<TextAsset> myLyricsFiles = new List<TextAsset>();
    public Text lyricsText;
   

    public Button PlayRandom;
    public Button muteButton;
    public Button nextButton;
    public Button unmuteButton;
    public Button previousButton;
    public Button volumeUpButton;
    public Button volumeDownButton;

    private int currentClipIndex = 0;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();


        muteButton.onClick.AddListener(Mute);
        unmuteButton.onClick.AddListener(Unmute);
        nextButton.onClick.AddListener(PlayNextSound);
        previousButton.onClick.AddListener(PlayPreviousSound);
        volumeUpButton.onClick.AddListener(VolumeUp);
        volumeDownButton.onClick.AddListener(VolumeDown);

        PlayNextSound();
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

        lyricsText.text = myLyricsFiles[currentClipIndex].text;

        StartCoroutine(PlayNextClipAfterDelay(myClip.length));
    }

    public void VolumeUp()
    {
        if (soundSource.volume < 1)
        {
            soundSource.volume += 0.1f;
            masterSoundVolume = soundSource.volume;
        }
    }

    public void VolumeDown()
    {
        if (soundSource.volume > 0)
        {
            soundSource.volume -= 0.1f;
            masterSoundVolume = soundSource.volume;
        }
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

        lyricsText.text = myLyricsFiles[currentClipIndex].text;

        StartCoroutine(PlayNextClipAfterDelay(myClip.length));
    }

    IEnumerator PlayNextClipAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayNextSound();
    }
}
