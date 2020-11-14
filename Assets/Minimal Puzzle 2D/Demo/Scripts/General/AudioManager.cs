using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour 
{
	public static AudioManager Instance;
	public AudioSource		BGM_Source;
	public AudioSource		BGS_Source;
	public AudioSource		SFX_Source;
	public AudioMixer		MixerComponent;

	// ----------------------------------------------
	public delegate void AudioRefreshDelegate();
	public static event AudioRefreshDelegate		AudioRefreshEvent;

	[SerializeField]
	private float masterVolume = 1f;
	public static float MasterVolume
	{
		get {return Instance.masterVolume; }
		set
		{ 
			Instance.masterVolume = Mathf.Clamp(value, 0f, 1f);
			Instance.MixerComponent.SetFloat("Master", Mathf.Lerp(-80, 0, Instance.masterVolume));
			if(AudioRefreshEvent != null) AudioRefreshEvent();
		}
	}

	void Awake()
	{
		Instance = this;
	}
	
	// ----------------------------------------------
	public static void PlayBGM (AudioClip _clip) 
	{
		if(_clip == null) return;
		Instance.BGM_Source.clip = _clip;
		Instance.BGM_Source.Play();
	}	

}
