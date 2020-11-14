using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(UI_ScreenComponent))]
public class UI_ScreenComponentEditor : Editor 
{	
	UI_ScreenComponent					source;
	Texture2D							showTexture;
	Texture2D							hideTexture;
	GUIStyle							buttonStyle = new GUIStyle();
	CanvasGroup							canvasComponent;
	GUIContent							content = new GUIContent();

	private void OnEnable()
	{
		// Get editor images styles
		source = (UI_ScreenComponent) target;
		showTexture =		Resources.Load<Texture2D>("Editor/Show");		
		hideTexture =		Resources.Load<Texture2D>("Editor/Hide");
		buttonStyle.alignment = TextAnchor.MiddleCenter;
		buttonStyle.imagePosition = ImagePosition.ImageAbove;
		buttonStyle.margin = new RectOffset(5,5,5,5);
		canvasComponent =	source.GetComponent<CanvasGroup>();
	}

	public override void OnInspectorGUI()
	{	
		buttonStyle = new GUIStyle(GUI.skin.button);
		buttonStyle.alignment = TextAnchor.MiddleCenter;
		buttonStyle.imagePosition = ImagePosition.ImageAbove;
		buttonStyle.margin = new RectOffset(5, 5, 5, 5);

		GUILayout.Space(6);

		GUILayout.BeginHorizontal();
		if(canvasComponent)
		{
			content = new GUIContent() {image = (canvasComponent.alpha > 0) ? hideTexture : showTexture, text = (canvasComponent.alpha > 0) ? "Hide" : "Show" };
			if(GUILayout.Button( content, buttonStyle, GUILayout.Width(50), GUILayout.Height(50) ) ) canvasComponent.alpha = (canvasComponent.alpha > 0) ? 0 : 1;
		}

		GUILayout.BeginVertical();		

			GUILayout.Space(6);
			GUILayout.Label("Screen Tag");
			source.ScreenTag = GUILayout.TextField(source.ScreenTag);		
			source.name = "[UI_Screen] " + source.ScreenTag;
			GUILayout.EndVertical();

		GUILayout.EndHorizontal();
		GUILayout.Space(10);
		EditorUtility.SetDirty(source);
	}

}

#endif

public class UI_ScreenComponent : UI 
{
	public string		ScreenTag;

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
			case MinimalPuzzleCallTypes.ShowScreen:
				if(ScreenTag == (string) _data)		Show();
				break;
			case MinimalPuzzleCallTypes.CloseScreen:
				if(ScreenTag == (string) _data)		Close();
				break;
		}
	}
}

