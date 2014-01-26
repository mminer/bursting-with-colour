using UnityEngine;
using System.Collections.Generic;

public static class ColorManager
{
	public static readonly Dictionary<LayerColor, Color> colors = new Dictionary<LayerColor, Color>()
	{
		{ LayerColor.Blue,   new Color(71  / 255f, 175 / 255f, 255 / 255f) },
		{ LayerColor.Green,  new Color(185 / 255f, 255 / 255f, 59  / 255f) },
		{ LayerColor.Red,    new Color(255 / 255f, 59  / 255f, 162 / 255f) },
		{ LayerColor.Yellow, new Color(255 / 255f, 171 / 255f, 59  / 255f) },
		{ LayerColor.Solid,  new Color(180 / 255f, 180 / 255f, 180 / 255f) },
	};
}
