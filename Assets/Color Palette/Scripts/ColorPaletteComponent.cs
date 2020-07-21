using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[DisallowMultipleComponent]
[CustomEditor(typeof(ColorPaletteComponent))]
public class ColorPaletteComponentEditor : Editor 
{	
	ColorPaletteComponent				source;
	GUIStyle							boxRounded = new GUIStyle();
	GUIStyle							boxRoundedSmall = new GUIStyle();
	GUIStyle							contourStyle = new GUIStyle();
	GUIStyle							contourStyleSmall = new GUIStyle();
	GUIStyle							labelStyle = new GUIStyle();
	Texture2D							logoTexture;

	void OnEnable()
	{
		// Get editor images styles
		boxRounded.normal.background = Resources.Load<Texture2D>("Editor/Fill");
		boxRounded.border = new RectOffset(8,8,8,8);
		boxRoundedSmall.normal.background = Resources.Load<Texture2D>("Editor/FillSmall");
		boxRoundedSmall.border = new RectOffset(4,4,4,4);
		contourStyle.normal.background = Resources.Load<Texture2D>("Editor/Border");
		contourStyle.border = new RectOffset(8, 8, 8, 8);
		contourStyleSmall.normal.background = Resources.Load<Texture2D>("Editor/BorderSmall");
		contourStyleSmall.border = new RectOffset(4, 4, 4, 4);
		labelStyle.normal.textColor = Color.white;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.fontSize = 9;
		labelStyle.alignment = TextAnchor.MiddleLeft;
		logoTexture = Resources.Load<Texture2D>("Editor/ColoPalette_Logo");
	}

	public override void OnInspectorGUI()
	{
		source = (ColorPaletteComponent) target;

		try
		{
			// Draw Logo.
			GUI.color = (source.UseColorPalette) ?  Color.white : Color.gray;
			GUILayout.Label( logoTexture );

			// Draw toggle.
			GUI.color = new Color(0f,0f,0f,0.18f);

			// Draw color list.
			GUILayout.BeginVertical(boxRounded, GUILayout.ExpandWidth(true));

			GUI.color = Color.white;

			GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.Height(18));
			GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
			GUILayout.Space(6);
			DrawToggleButton(ColorPaletteComponent.CurrentPaletteData);
			GUILayout.Label(source.PaletteName, labelStyle, GUILayout.ExpandWidth(true), GUILayout.Height(26));
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();

			if (source.UseColorPalette) DrawColorList(source.ColorPaletteIndex);

			GUILayout.EndVertical();

		}
		catch
		{
			source.LoadCurrentPaletteData();
		}

		EditorUtility.SetDirty(source);
	}


	void DrawToggleButton(ColorPaletteCustomData.PaletteClass _data)
	{
		labelStyle.normal.textColor = Color.gray;

		// Toggle Area
		GUI.color = Color.clear;
		GUILayout.Box("", GUILayout.Width(18), GUILayout.Height(18));
		var area = GUILayoutUtility.GetLastRect();

		// Fill
		GUI.color = Color.white;
		if( source.UseColorPalette )
		{
			GUI.Box(new Rect(area.x + 4, area.y + 4, area.width - 8, area.height - 8), "", boxRoundedSmall);
			ColorPaletteComponent.CurrentPaletteData = _data;
			labelStyle.normal.textColor = Color.white;
		}

		// Border
		GUI.color = Color.gray;
		if (GUI.Button(area, "", contourStyleSmall))
		{
			source.UseColorPalette = !source.UseColorPalette;
			source.OnRefreshColorPalette();
			EditorSceneManager.MarkAllScenesDirty();
		}
		GUI.color = Color.white;
	}

	void DrawColorList(int _index)
	{	
		GUILayout.Space(2);
		GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));

		GUILayout.Space(8);

		source.LoadCurrentPaletteData();

		if(ColorPaletteComponent.CurrentPaletteData == null)  return;		

		// Draw all colors in palette.
		for (int i = 0; i < ColorPaletteComponent.CurrentPaletteData.ColorList.Count; i++)
		{
			GUI.color = ColorPaletteComponent.CurrentPaletteData.ColorList[i] + new Color(0,0,0,1f);
			GUILayout.Space(6);
			if (GUILayout.Button("", boxRounded, GUILayout.Width(24), GUILayout.Height(24)))
			{
				source.ColorPaletteIndex = i;
				source.OnRefreshColorPalette();
				EditorSceneManager.MarkAllScenesDirty();
			}
			
			if (i == _index) DrawOverSelection();			
		}	
		
		GUILayout.EndHorizontal();
		GUILayout.Space(8);
	}

	void DrawOverSelection()
	{		
		GUI.color = Color.white;			
		var rect = GUILayoutUtility.GetLastRect();
		rect.width += 6;
		rect.height += 6;
		rect.x -= 3;
		rect.y -= 3;
		GUI.Box(rect, "", contourStyle);			
	}

}

[ExecuteInEditMode]
public class ColorPaletteComponent : MonoBehaviour 
{
	public static ColorPaletteCustomData.PaletteClass	CurrentPaletteData;

	// ----------------------------------------------------------------------- 2D FIELDS
	public Renderer					FieldRenderer;
	public Image					FieldGraphic;
	public RawImage					FieldRawGraphic;
	public Text						FieldLabel;
	public Camera					FieldCamera;

	// ----------------------------------------------------------------------- VARIABLES
	public bool						UseColorPalette = true;
	public int						ColorPaletteIndex = 0;
	public Color					CurrentColor = new Color(1,1,1,1);
	public string					PaletteName = "";

	// ----------------------------------------------------------------------- COLOR PALETTE
	public void Awake()
	{
		if(CurrentPaletteData == null) LoadCurrentPaletteData();

		FieldRenderer =		GetComponent<Renderer>();
		FieldGraphic =		GetComponent<Image>();
		FieldRawGraphic =	GetComponent<RawImage>();
		FieldLabel =		GetComponent<Text>();
		FieldCamera =		GetComponent<Camera>();
		OnRefreshColorPalette();
	}

	void OnEnable()
	{
		ColorPaletteWindow.RefreshColorPaletteEvent += OnRefreshColorPalette;
	}

	void OnDisable()
	{
		ColorPaletteWindow.RefreshColorPaletteEvent -= OnRefreshColorPalette;
	}	

	// --------------------------------------------------------
	public void LoadCurrentPaletteData()
	{
		if(CurrentPaletteData == null)
		{
			// Collect current palette data.
			var palettePack =					EditorPrefs.GetInt("ColorPalette_PalettePack", 0);
			var paletteIndex =					EditorPrefs.GetInt("ColorPalette_PaletteIndex", 0);
			var customDataPacks =				Resources.LoadAll<ColorPaletteCustomData>("");
			if(customDataPacks.Length > palettePack)
			CurrentPaletteData =				customDataPacks[palettePack].PaletteList[paletteIndex];
		}
	}
	// --------------------------------------------------------

	public void OnRefreshColorPalette()
	{
		if(!UseColorPalette) return;
		
		if(CurrentPaletteData != null)
		{
			if(ColorPaletteIndex < CurrentPaletteData.ColorList.Count)
			{
				CurrentColor = CurrentPaletteData.ColorList[ColorPaletteIndex] + new Color(0, 0, 0, 1);
				PaletteName = CurrentPaletteData.Name;
				if (!EditorApplication.isPlayingOrWillChangePlaymode) EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
			}
		}

		RefreshColors();		
	}

	public void RefreshColors()
	{
		if(FieldGraphic) {		FieldGraphic.color = CurrentColor;	EditorUtility.SetDirty(FieldGraphic); }
		if(FieldLabel) {		FieldLabel.color = CurrentColor;	EditorUtility.SetDirty(FieldLabel); }
		if(FieldRawGraphic) {	FieldRawGraphic.color = CurrentColor; EditorUtility.SetDirty(FieldRawGraphic); }
		if(FieldCamera) 
		{		
			FieldCamera.backgroundColor = CurrentColor;  
			//RenderSettings.ambientLight = CurrentColor;	
		}

		if (FieldRenderer)
		{
			var mat = new Material(FieldRenderer.sharedMaterial);
			mat.color = CurrentColor;
			FieldRenderer.material = mat;
		}
	}
	// -----------------------------------------------------------------------	
	
}

#endif