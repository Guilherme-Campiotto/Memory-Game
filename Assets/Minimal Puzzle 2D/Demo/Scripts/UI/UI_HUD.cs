using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_HUD : MonoBehaviour 
{
	public Text		FieldCurrentLevel;

	void OnEnable()
	{
		MinimalPuzzle2D.TriggerEvent += OnTriggerEvent;
	}

	void OnDisable()
	{
		MinimalPuzzle2D.TriggerEvent -= OnTriggerEvent;
	}

	private void OnTriggerEvent(MinimalPuzzleCallTypes _actionType, object _data = null)
	{
		switch (_actionType)
		{
			case MinimalPuzzleCallTypes.ChangeLevel:
				FieldCurrentLevel.text = (MinimalPuzzle2D.CurrentLevel).ToString();
				break;
			case MinimalPuzzleCallTypes.NextLevel:
				MinimalPuzzle2D.CurrentLevel++;
				break;
			case MinimalPuzzleCallTypes.PrevLevel:
				MinimalPuzzle2D.CurrentLevel--;
				break;
		}
	}


	
}
