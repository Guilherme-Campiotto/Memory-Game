using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Color Palette", menuName = "PDJ/Create New Color Palette", order = 1001)]
public class ColorPaletteCustomData : ScriptableObject
{
	public ColorPaletteCustomData()
	{
		PaletteList = new List<PaletteClass>() { new PaletteClass() { Name = "_untitled_", ColorList = new List<Color>() { Color.white }  } };
	}

	[System.Serializable]
	public class PaletteClass
	{
		public string				Name;
		public List<Color>			ColorList = new List<Color>();
	}

	public List<PaletteClass>		PaletteList = new List<PaletteClass>();
}
