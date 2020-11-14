using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Minimal 2D Asset Pack", menuName = "PDJ/Create New Minimal 2D Asset Pack", order = 1000)]
public class MinimalPuzzle2DCustomData : ScriptableObject
{
	public MinimalPuzzle2DCustomData()
	{
		AssetList = new List<AssetClass>() { new AssetClass() { ImageType = UnityEngine.UI.Image.Type.Sliced  } };
	}

	[System.Serializable]
	public class AssetClass
	{
		public Sprite						Graphic;
		public UnityEngine.UI.Image.Type	ImageType;
	}

	public List<AssetClass>			AssetList = new List<AssetClass>();
}