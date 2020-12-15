using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource audioSourceEffects;
    public static SoundController instance { get; private set; } = null;
    public AudioClip[] soundtrack;
    public int numberOfSongs;
    public int numberOfCurrentSong = 0;
    public bool nextSongQueue = false;
    public float timePlayNextSong = 2f;

    public Slider musicSlider;
    public Slider soundEffectSlider;

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

        PlayMusicOnce(soundtrack[numberOfCurrentSong], 0.8f);

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
        PlayMusicOnce(soundtrack[numberOfCurrentSong], 0.8f);
        nextSongQueue = false;
    }

    public void PlayAudioOnce(AudioClip clip, float volume = 2f)
    {
        audioSourceEffects.PlayOneShot(clip, volume);
    }

    public void PlayMusicOnce(AudioClip clip, float volume = 2f)
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

    /*public void ChangeAudioStatus()
    {
        audioSource.mute = !audioSource.mute;
    }*/

    public void SetVolumeMusic(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        audioSource.volume = volume;
    }

    public void SetVolumeSoundEffects(float volume)
    {
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
        audioSourceEffects.volume = volume;
    }

    public void LoadGameVolume()
    {

        musicSlider = GameObject.Find("SliderMusic").GetComponent<Slider>();
        musicSlider.onValueChanged.AddListener(SetVolumeMusic);
        soundEffectSlider = GameObject.Find("SliderSoundEffects").GetComponent<Slider>();
        soundEffectSlider.onValueChanged.AddListener(SetVolumeSoundEffects);

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        } else
        {
            musicSlider.value = 0.7f;
            audioSource.volume = 0.7f;
            PlayerPrefs.SetFloat("MusicVolume", 0.7f);
        }

        if (PlayerPrefs.HasKey("SoundEffectsVolume"))
        {
            soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume");
            audioSourceEffects.volume = PlayerPrefs.GetFloat("SoundEffectsVolume");
        }
        else
        {
            soundEffectSlider.value = 0.7f;
            audioSourceEffects.volume = 0.7f;
            PlayerPrefs.SetFloat("SoundEffectsVolume", 0.7f);
        }
    }
}
