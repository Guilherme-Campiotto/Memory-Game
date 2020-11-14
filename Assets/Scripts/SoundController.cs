using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSource;

    public static SoundController instance { get; private set; } = null;
    public AudioClip[] soundtrack;
    public int numberOfSongs;
    public int numberOfCurrentSong = 0;
    public bool nextSongQueue = false;
    public float timePlayNextSong = 2f;
    string sound;

    void Awake()
    {
        if (gameObject.CompareTag("SoundController") && instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else if (gameObject.CompareTag("SoundController"))
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        numberOfSongs = soundtrack.Length - 1;

        sound = PlayerPrefs.GetString("Sound");
        if (sound == "Off")
        {
            audioSource.mute = true;
        }
        else
        {
            PlayAudioOnce(soundtrack[numberOfCurrentSong], 0.8f);
        }

    }

    private void Update()
    {
        if(!audioSource.isPlaying && !nextSongQueue)
        {
            numberOfCurrentSong++;
            if(numberOfCurrentSong > numberOfSongs)
            {
                numberOfCurrentSong = 0;
            }

            StartCoroutine(PlayNextSong());
        }
    }

    IEnumerator PlayNextSong()
    {
        nextSongQueue = true;
        yield return new WaitForSeconds(timePlayNextSong);
        PlayAudioOnce(soundtrack[numberOfCurrentSong], 0.8f);
        nextSongQueue = false;
    }

    public void PlayAudioOnce(AudioClip clip, float volume = 2f)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    public void MuteAudio()
    {
        audioSource.mute = true;
    }

    public void PlayAudio()
    {
        audioSource.mute = false;
    }

    public void PlayWithLoop(AudioClip music)
    {
        audioSource.Stop();
        audioSource.clip = music;
        audioSource.Play();
    }

}
