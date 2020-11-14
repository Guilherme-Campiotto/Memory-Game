using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UI : MonoBehaviour 
{
	CanvasGroup		CanvasComponent;
	Coroutine		canvasRoutine;

	public virtual void Awake () 
	{
		CanvasComponent = GetComponent<CanvasGroup>();
		FadeOut();
	}

	public virtual void Show()
	{
		FadeIn(0.24f);
	}
	
	public virtual void Close()
	{
		FadeOut(0.24f);
	}

	public void FadeIn(float _time = 0.01f)
	{
		CanvasComponent.interactable = true;
		CanvasComponent.blocksRaycasts = true;
		if(canvasRoutine != null) StopCoroutine(canvasRoutine);
		canvasRoutine = StartCoroutine( CanvasAlphaRoutine(1f, _time) );
	}

	public void FadeOut(float _time = 0.01f)
	{
		CanvasComponent.interactable = false;
		CanvasComponent.blocksRaycasts = false;
		if(canvasRoutine != null) StopCoroutine(canvasRoutine);
		canvasRoutine = StartCoroutine( CanvasAlphaRoutine(0f, _time) );
	}

	IEnumerator CanvasAlphaRoutine(float _to, float _timer)
	{
		var value = 0f;
		var fromTime = Time.time;
		var targetTime = Time.time + _timer;
		var fromAlpha = CanvasComponent.alpha;
		var targetAlpha = _to;

		while(value < 1f)
		{
			value = Mathf.InverseLerp( fromTime, targetTime, Time.time);
			CanvasComponent.alpha = Mathf.Lerp(fromAlpha, targetAlpha, value);
			yield return new WaitForEndOfFrame();
		}
	}

}
