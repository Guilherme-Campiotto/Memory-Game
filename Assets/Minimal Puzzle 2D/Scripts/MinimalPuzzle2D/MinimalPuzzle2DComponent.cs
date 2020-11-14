using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MinimalPuzzle2DComponent))]
public class MinimalPuzzle2DComponentEditor : Editor 
{	
	MinimalPuzzle2DComponent			source;
	Texture2D							actionTexture;
	Texture2D							imageTexture;
	Texture2D							changeImageTexture;
	Texture2D							resizeTexture;
	GUIStyle							buttonStyle = new GUIStyle();
	GUIStyle							boxRounded = new GUIStyle();
	GUIContent							content = new GUIContent();

	private void OnEnable()
	{
		// Get editor images styles
		source = (MinimalPuzzle2DComponent) target;
		actionTexture =					Resources.Load<Texture2D>("Editor/Action");		
		imageTexture =					Resources.Load<Texture2D>("Editor/Image");
		changeImageTexture =			Resources.Load<Texture2D>("Editor/ChangeImage");
		resizeTexture =					Resources.Load<Texture2D>("Editor/Resize");
		boxRounded.normal.background =	Resources.Load<Texture2D>("Editor/EditorButton");
		boxRounded.border = new RectOffset(8,8,8,8);
	}

	public override void OnInspectorGUI()
	{
		buttonStyle = new GUIStyle(GUI.skin.button);
		buttonStyle.alignment = TextAnchor.MiddleCenter;
		buttonStyle.imagePosition = ImagePosition.ImageAbove;
		buttonStyle.margin = new RectOffset(5, 5, 5, 5);

		// ----------------------------------------------
		GUILayout.Space(6);
		GUI.color = new Color(0,0,0,0.18f);
		GUILayout.BeginVertical(boxRounded, GUILayout.Height(40));

		GUI.color = Color.white;
		content = new GUIContent( "  Action Event", actionTexture );
		GUILayout.Label(content);

		EditorGUI.BeginChangeCheck ();

		source.OnClickAction = (MinimalPuzzleCallTypes) EditorGUILayout.EnumPopup(source.OnClickAction);

		switch (source.OnClickAction)
		{
			case MinimalPuzzleCallTypes.ShowScreen:
			case MinimalPuzzleCallTypes.CloseScreen:
				source.ScreenTagIndex = EditorGUILayout.Popup(source.ScreenTagIndex, source.ScreenList.ToArray() );
				break;
		}

		if (EditorGUI.EndChangeCheck ()) 
		{
			source.ScreenList.Clear();
			var screenComponentList = Resources.FindObjectsOfTypeAll<UI_ScreenComponent>();
			foreach (var screen in screenComponentList) if(screen.ScreenTag != "") source.ScreenList.Add(screen.ScreenTag);
			
			if(source.OnClickAction == MinimalPuzzleCallTypes.None) RemoveButtonAnimation(); else AddButtonAnimation();
		}

		GUILayout.Space(6);

		GUILayout.EndVertical();

		// ----------------------------------------------
		GUILayout.Space(6);
		
		GUI.color = new Color(0,0,0,0.18f);
		GUILayout.BeginVertical(boxRounded);
		GUI.color = Color.white;
		content = new GUIContent( "  Image", imageTexture );
		GUILayout.Label(content);

		GUILayout.BeginHorizontal();

		// Open a new window to get another image and change current selection.
		content = new GUIContent( "Change Image", changeImageTexture );
		if(GUILayout.Button(content, buttonStyle, GUILayout.Width(110), GUILayout.Height(58)))
		{
			if(MinimalPuzzle2DWindow.floatingWindow) MinimalPuzzle2DWindow.floatingWindow.Close();
			var newWindow = ScriptableObject.CreateInstance<MinimalPuzzle2DWindow>();
			newWindow.ShowWindow(source);
			MinimalPuzzle2DWindow.floatingWindow = newWindow;
		}

		content = new GUIContent( "Reset Size", resizeTexture );
		if(GUILayout.Button(content, buttonStyle, GUILayout.Width(90), GUILayout.Height(58)))	source.RefreshSize();

		GUILayout.Space(3);
		GUILayout.BeginVertical();
		source.FlipX =				GUILayout.Toggle(source.FlipX, "Flip X");
		source.FlipY =				GUILayout.Toggle(source.FlipY, "Flip Y");
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
		GUILayout.Space(6);
		GUILayout.EndVertical();

		GUILayout.Space(10);

		EditorUtility.SetDirty(source);
	}

	void AddButtonAnimation()
	{
		if (source.GetComponent<Selectable>() == null) source.gameObject.AddComponent<Selectable>();
	}

	void RemoveButtonAnimation()
	{
		var component = source.GetComponent<Selectable>();
		if (component != null)
		{
			DestroyImmediate(component);
			EditorGUIUtility.ExitGUI();
		}
	}

}

#endif

public class MinimalPuzzle2DComponent : MonoBehaviour, IPointerClickHandler
{
	public MinimalPuzzleCallTypes	OnClickAction;
	public int						ScreenTagIndex;
	public List<string>				ScreenList = new List<string>();
	public Image					FieldGraphic;
	[SerializeField]
	private bool					flipX = false;
	public bool						FlipX { get{ return flipX; } set{ flipX = value; RefreshFlip(); }}
	[SerializeField]
	private bool					flipY = false;
	public bool						FlipY { get{return flipY; } set{ flipY = value; RefreshFlip(); }}

	public void OnPointerClick(PointerEventData eventData)
	{
		object sender = null;
		try
		{
			switch (OnClickAction)
			{
				case MinimalPuzzleCallTypes.ShowScreen:
				case MinimalPuzzleCallTypes.CloseScreen:
					if(ScreenList.Count > ScreenTagIndex)	sender = ScreenList[ScreenTagIndex];
					break;
			}
		}
		catch { }

		MinimalPuzzle2D.Call( OnClickAction , sender);
	}

	// Setup element.
	public void SetAttributes(MinimalPuzzle2DCustomData.AssetClass _data)
	{
		if(FieldGraphic) FieldGraphic.type =		_data.ImageType;
		if(FieldGraphic) FieldGraphic.sprite =		_data.Graphic;
	}

	// Flip graphic.
	public void RefreshFlip()
	{
		if(FieldGraphic) FieldGraphic.rectTransform.localScale = new Vector3((FlipX)?-1:1,(FlipY)?-1:1,1);
	}

	// Set dafault size, based on sprite size.
	public void RefreshSize()
	{
		if(FieldGraphic) FieldGraphic.GetComponent<RectTransform>().sizeDelta = FieldGraphic.sprite.rect.size;
	}
}
