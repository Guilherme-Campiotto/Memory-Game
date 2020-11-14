using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_AudioComponent : MonoBehaviour 
{
	[HideInInspector]
	public Sprite		GraphicOn;
	[HideInInspector]
	private Image		FieldGraphic;

	public Sprite		GraphicOff;

	void Awake()
	{
		FieldGraphic = GetComponent<Image>();
		GraphicOn = FieldGraphic.sprite;
	}

	void Start()
	{
		OnAudioRefresh();
	}

	void OnEnable()
	{
		AudioManager.AudioRefreshEvent += OnAudioRefresh;
	}

	void OnDisable()
	{
		AudioManager.AudioRefreshEvent -= OnAudioRefresh;
	}

	private void OnAudioRefresh()
	{
		if(FieldGraphic) FieldGraphic.sprite = (AudioManager.MasterVolume > 0) ? GraphicOn : GraphicOff;
	}


}
