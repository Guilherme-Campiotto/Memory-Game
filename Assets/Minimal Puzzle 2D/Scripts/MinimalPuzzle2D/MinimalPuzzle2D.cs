using UnityEngine;

public class MinimalPuzzle2D : MonoBehaviour
{ 
	public static MinimalPuzzle2D			Instance;

	// Call Events.
	// ----------------------------------------------------------------------
	public delegate void TriggerDelegate(MinimalPuzzleCallTypes _actionType, object _data = null);
	public static event TriggerDelegate		TriggerEvent;
	
	// Variables
	// ----------------------------------------------------------------------
	public AudioClip			BGMClip;

	// Set the auto begin Backgound Music (BGM) clip
	[SerializeField]
	private int						currentLevel = 1;
	public static int				CurrentLevel
	{
		get {return Instance.currentLevel; }
		set
		{
			Instance.currentLevel = Mathf.Clamp(value, 1, 99);
			Call(MinimalPuzzleCallTypes.ChangeLevel);
		}
	}

	// ----------------------------------------------------------------------
	void Awake()
	{
		// Singleton
		Instance = this;
	}

	void Start()
	{		
		SetupGame();
	}
	// ----------------------------------------------------------------------

	/// <summary>
	/// Setup game config on start.
	/// </summary>
	void SetupGame()
	{
		// Load data in Playerprefs
		Load();

		// Play auto begin Backgound Music (BGM)
		AudioManager.PlayBGM( BGMClip );
	}

	// ----------------------------------------------------------------------
	/// <summary>
	/// Call event.
	/// </summary>
	/// <param name="_actionType">Event type</param>
	/// <param name="_data">Extra info like "screen tag" for example.</param>
	public static void Call(MinimalPuzzleCallTypes _actionType, object _data = null) 
	{
		switch (_actionType)
		{
			// Toggle audio master volume.
			case MinimalPuzzleCallTypes.AudioMasterToggle:
				AudioManager.MasterVolume = (AudioManager.MasterVolume > 0f) ? 0f : 1f;
				break;
			// Quit the game.
			case MinimalPuzzleCallTypes.QuitGame:
				Application.Quit();
				break;
			// Save the game.
			case MinimalPuzzleCallTypes.SaveGame:
				Save();
				break;
		}

		// Send event to listeners.
		if(TriggerEvent != null) TriggerEvent(_actionType, _data);
	}

	/// <summary>
	/// Load data in database (Playerprefs).
	/// </summary>
	void Load()
	{		
		CurrentLevel =					PlayerPrefs.GetInt("CurrentLevel", 1);
		AudioManager.MasterVolume =		PlayerPrefs.GetFloat("AudioMasterVolume", 1f);
	}

	/// <summary>
	/// Save data in database (Playerprefs).
	/// </summary>
	public static void Save()
	{
		PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
		PlayerPrefs.SetFloat("AudioMasterVolume", AudioManager.MasterVolume);
	}

}

// Call event types.
// ----------------------------------------------------------------------
public enum MinimalPuzzleCallTypes
{
	None = 0,
	QuitGame = 1,	
	LoadGame = 2,	
	SaveGame = 3,	
	ShowScreen = 10,
	CloseScreen = 11,
	ChangeLevel = 20,
	NextLevel = 21,
	PrevLevel = 22,
	AudioMasterToggle = 30,
}


