using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;

public class MinimalPuzzle2DWindow : EditorWindow 
{
	public MinimalPuzzle2DComponent			Source;
	public MinimalPuzzle2DCustomData[]		CustomData;
	Vector2									scroll;
	public static MinimalPuzzle2DWindow		floatingWindow = null;
	GUIStyle								buttonStyle = new GUIStyle();
	GUIContent								content = new GUIContent();
	Texture2D								logoTexture;
	Texture2D								textTexture;
	Texture2D								screenTexture;

	private int								packIndex = 0;
	public int								PackIndex
	{
		get { return packIndex; }
		set
		{
			if(packIndex != value)
			{
				packIndex = value;
				EditorPrefs.SetInt("MinimalPuzzle2D_PackIndex", packIndex);
				RefreshPacks();
			}
		}
	}

	private float							thumbSize = 32f;
	public float							ThumbSize
	{
		get { return thumbSize; }
		set
		{
			thumbSize = value;
			EditorPrefs.SetFloat("MinimalPuzzle2D_ThumbSize", thumbSize);
		}
	}

	// -----------------------------------------
	[MenuItem("Window/PDJ/Minimal 2D Assets Pack")]
    static void Init()
    {
		var window = GetWindow<MinimalPuzzle2DWindow>();
		window.Show();
    }

	private void OnEnable()
	{
		// Load Data
		logoTexture =		Resources.Load<Texture2D>("Editor/MinimalPuzzle2D_Logo");		
		textTexture =		Resources.Load<Texture2D>("Editor/Text");
		screenTexture =		Resources.Load<Texture2D>("Editor/Screen");
		thumbSize =			EditorPrefs.GetFloat("MinimalPuzzle2D_ThumbSize", 32);
		packIndex =			EditorPrefs.GetInt("MinimalPuzzle2D_PackIndex", 0);	

		// Update Packs.
		RefreshPacks();

		content = new GUIContent(){ image = (Texture) Resources.Load("Editor/MinimalPuzzle2D_Icon"), text = "Minimal 2D"};
		this.titleContent = content;
	}

	public void ShowWindow(MinimalPuzzle2DComponent _source)
	{
		// Load custom palette data
		Source = _source;
		Init();
	}

	void RefreshPacks()
	{
		Resources.UnloadUnusedAssets();

		// Find all assets pack
		CustomData = Resources.LoadAll<MinimalPuzzle2DCustomData>("");

		// Avoid delete package
		packIndex = Mathf.Clamp(packIndex, 0, CustomData.Length);
	}

	public void OnGUI()
	{
		if (EditorApplication.isPlayingOrWillChangePlaymode) if (Source != null)	this.Close();

		if (EditorApplication.isPlaying)
		{
			//if (Source != null) this.Close();
			return;
		}

		if(CustomData == null || CustomData.Length <= 0)
		{
			GUILayout.Label("Data not found! Refresh window or create one using:  Create/PDJ/Create New Minimal 2D Asset Pack... ");
			return;
		}

		try
		{
			buttonStyle = new GUIStyle(GUI.skin.button);
			buttonStyle.alignment = TextAnchor.MiddleCenter;
			buttonStyle.imagePosition = ImagePosition.ImageAbove;
			buttonStyle.margin = new RectOffset(5, 5, 5, 5);

			// Draw Toolbar.
			if(Source == null)	DrawToolbar();			

			// Pack list
			var options = new List<string>();
			foreach (var item in CustomData) options.Add(item.name);
			PackIndex = EditorGUILayout.Popup(PackIndex, options.ToArray());

			// Custom size thumbs
			ThumbSize = EditorGUILayout.Slider(ThumbSize, 32f, 128f);		

			var xCount = Mathf.Clamp(Mathf.FloorToInt( (Screen.width / ThumbSize) * 0.88f ), 1, 999);

			scroll = EditorGUILayout.BeginScrollView(scroll);
			GUILayout.BeginVertical();
			var i = 0;
			while(i < CustomData[PackIndex].AssetList.Count)
			{
				// Auto grid tiles
				GUILayout.BeginHorizontal();
				for (int n = 0; n < xCount; n++)
				{
					if(CustomData[PackIndex].AssetList.Count <= i) break;
					DrawGraphicThumb( CustomData[PackIndex].AssetList[i] );
					i++;
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}
		catch
		{			
			PackIndex = 0;
			EditorGUIUtility.ExitGUI();
		}
		
	}

	// ------------------------------------------------------------------ TOOLBAR
	void DrawToolbar()
	{
		GUILayout.BeginHorizontal();
		Vector2 size = new Vector2(80, 44);

		GUILayout.Label( logoTexture );

		// -----------------------------
		content = new GUIContent( "Add Text", textTexture );
		if(GUILayout.Button(content, buttonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y)))
		{
			var parent = GetParent();
			var obj = Resources.Load<GameObject>("Minimal2D/Prefabs/[UI_Text]");
			var clone = Editor.Instantiate(obj);
			clone.name = obj.name;
			if(parent)	clone.transform.SetParent (parent.transform, false);
			Selection.activeObject = clone;		
		}

		// -----------------------------
		content = new GUIContent( "Add Screen", screenTexture );
		if(GUILayout.Button(content, buttonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y)))
		{
			var parent = GetParent();
			var obj = Resources.Load<GameObject>("Minimal2D/Prefabs/[UI_Screen]");
			var clone = Editor.Instantiate(obj);
			clone.name = obj.name;
			if(parent)	clone.transform.SetParent (parent.transform, false);
			Selection.activeObject = clone;			
		}

		GUILayout.EndHorizontal();
	}

	// Get the parente inside canvas if exists.
	GameObject GetParent()
	{
		// Get canvas parent
		var c = GameObject.FindObjectOfType<Canvas>();
		if(c == null) c = CreateNewCanvas();
			
		var canvas = c.gameObject;

		if(canvas == null) return null;

		if(Selection.activeGameObject)
		{
			// Set parent in current selection (only inside canvas).
			if(Selection.activeGameObject.GetComponentInParent<Canvas>())
			{
				return Selection.activeGameObject;
			}
			else
			{
				return canvas;
			}
		}
		else
		{
			return canvas;
		}
	}

	Canvas CreateNewCanvas()
	{
		var newCanvas = new GameObject("Canvas");
		var c = newCanvas.AddComponent<Canvas>();
		c.renderMode = RenderMode.ScreenSpaceOverlay;
		c.pixelPerfect = true;
		newCanvas.AddComponent<UnityEngine.UI.CanvasScaler>();
		newCanvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
		var newEventSystem = new GameObject("EventSystem");
		newEventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
		newEventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
		return c;
	}

	// Draw group box
	void DrawGraphicThumb(MinimalPuzzle2DCustomData.AssetClass _data)
	{
		buttonStyle = EditorStyles.centeredGreyMiniLabel;

		// Force White draw
		GUI.color = Color.white;
		if(GUILayout.Button(_data.Graphic.texture, buttonStyle, GUILayout.Width(ThumbSize), GUILayout.Height(ThumbSize)))
		{
			var parent = GetParent();

			// Replace element
			if(Source)
			{
				Source.SetAttributes(_data);
				EditorUtility.SetDirty(Selection.activeGameObject);
				this.Close();
				return;
			}			

			// Create a new UI element
			// -----------------------
			var obj = Resources.Load<GameObject>("Minimal2D/Prefabs/[UI_Image]");
			var clone = Editor.Instantiate(obj);
			clone.name = obj.name;
			if(parent)	clone.transform.SetParent (parent.transform, false);
			Selection.activeObject = clone;
			clone.GetComponent<MinimalPuzzle2DComponent>().SetAttributes(_data);
			clone.GetComponent<MinimalPuzzle2DComponent>().RefreshSize();

			// Auto add Color Palette on creation.
			var type = System.Type.GetType("ColorPaletteComponent");
			if(type != null)	clone.AddComponent(type);
		}
	}
}

#endif