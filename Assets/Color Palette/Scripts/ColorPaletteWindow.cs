using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;

public class ColorPaletteWindow : EditorWindow 
{
	public static Action				RefreshColorPaletteEvent;

	public ColorPaletteCustomData[]		CustomData;
	Vector2								scroll;
	GUIStyle							boxRounded = new GUIStyle();
	GUIStyle							boxRoundedSmall = new GUIStyle();
	GUIStyle							contourStyle = new GUIStyle();
	GUIStyle							contourStyleSmall = new GUIStyle();
	GUIStyle							labelStyle = new GUIStyle();
	Texture2D							logoTexture;
	int									palettePack;

	private int							packIndex = 0;
	public int							PackIndex
	{
		get { return packIndex; }
		set
		{
			packIndex = value;
			EditorPrefs.SetInt("ColorPalette_PackIndex", packIndex);
			RefreshPacks();
		}
	}

	private int							paletteIndex = 0;
	public int							PaletteIndex
	{
		get { return paletteIndex; }
		set
		{
			paletteIndex = value;
			palettePack = PackIndex;
			EditorPrefs.SetInt("ColorPalette_PaletteIndex", paletteIndex);		
			EditorPrefs.SetInt("ColorPalette_PalettePack", palettePack);
		}
	}

	[MenuItem("Window/PDJ/Color Palette")]
    static void Init()
    {
		var window = GetWindow<ColorPaletteWindow>();
		window.Show();
    }

	private void OnEnable()
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

		// Load Data
		packIndex =		EditorPrefs.GetInt("ColorPalette_PackIndex", 0);		
		palettePack =	EditorPrefs.GetInt("ColorPalette_PalettePack", -1);
		paletteIndex =	EditorPrefs.GetInt("ColorPalette_PaletteIndex", 0);

		RefreshPacks();

		var content = new GUIContent(){ image = (Texture) Resources.Load("Editor/ColoPalette_Icon"), text = "Color Palette"};
		this.titleContent = content;
	}

	public void ShowWindow()
	{
		Init();
	}

	void RefreshPacks()
	{
		Resources.UnloadUnusedAssets();

		// Find all assets pack
		CustomData = Resources.LoadAll<ColorPaletteCustomData>("");

		// Avoid delete package
		packIndex = Mathf.Clamp(packIndex, 0, CustomData.Length);
	}

	public void OnGUI()
	{
		if(CustomData == null || CustomData.Length <= 0)
		{
			GUILayout.Label("Data not found! Refresh window or create one using:  Create/PDJ/Create New Palette... ");
			return;
		}

		try
		{
			// Draw logo.
			GUILayout.Label(logoTexture);

			EditorGUI.BeginChangeCheck ();
			// Draw popup options.
			var options = new List<string>();
			foreach (var item in CustomData) options.Add(item.name);
			PackIndex = EditorGUILayout.Popup(PackIndex, options.ToArray());

			if (EditorGUI.EndChangeCheck ()) 
			{
				RefreshPacks();
			}

			// Draw colors list.
			scroll = GUILayout.BeginScrollView(scroll);
			GUILayout.BeginVertical();
			for (int i = 0; i < CustomData[PackIndex].PaletteList.Count; i++)
			{
				GUI.color = new Color(0f,0f,0f,0.18f);
				GUILayout.Space(6);

				GUILayout.BeginVertical(boxRounded, GUILayout.ExpandWidth(true));
					GUI.color = Color.white;
					GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.Height(18));
					GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
					GUILayout.Space(6);
					DrawToggleButton( i, CustomData[PackIndex].PaletteList[i] );
					GUILayout.Label(CustomData[PackIndex].PaletteList[i].Name, labelStyle, GUILayout.ExpandWidth(true), GUILayout.Height(26));
					GUILayout.EndHorizontal();				
					GUILayout.EndVertical();
					DrawPaletteBox( CustomData[PackIndex].PaletteList[i] );
				GUILayout.EndVertical();
			}
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		}
		catch
		{
			PackIndex = 0;
			EditorGUIUtility.ExitGUI();
		}
	}

	void DrawToggleButton(int _index, ColorPaletteCustomData.PaletteClass _data)
	{
		labelStyle.normal.textColor = Color.gray;

		// Toggle Area
		GUI.color = Color.clear;
		GUILayout.Box("", GUILayout.Width(18), GUILayout.Height(18));
		var area = GUILayoutUtility.GetLastRect();

		// Fill
		GUI.color = Color.white;
		if( PaletteIndex == _index && palettePack == PackIndex)
		{
			GUI.Box(new Rect(area.x + 4, area.y + 4, area.width - 8, area.height - 8), "", boxRoundedSmall);
			ColorPaletteComponent.CurrentPaletteData = _data;
			labelStyle.normal.textColor = Color.white;
		}

		// Border
		GUI.color = Color.gray;
		if (GUI.Button(area, "", contourStyleSmall))
		{
			ColorPaletteComponent.CurrentPaletteData = _data;
			PaletteIndex = _index;
			if (RefreshColorPaletteEvent != null) RefreshColorPaletteEvent();
		}
		GUI.color = Color.white;

	}

	// Draw Palette group box
	void DrawPaletteBox(ColorPaletteCustomData.PaletteClass _data)
	{		
		GUILayout.Space(2);
		GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));

		GUILayout.Space(24);

		for (int i = 0; i < _data.ColorList.Count; i++)
		{
			DrawColorThumb( _data.ColorList[i] );
			GUILayout.Space(2);
		}

		GUILayout.EndHorizontal();
		GUILayout.Space(8);

	}

	// Draw colors list
	void DrawColorThumb(Color _color)
	{
		GUI.color = _color + new Color(0,0,0,1);
		GUILayout.Box("", boxRounded, GUILayout.Width(20), GUILayout.Height(20));
	}	
}

#endif