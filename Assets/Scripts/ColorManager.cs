using UnityEngine;
using System.Collections.Generic;

public static class ColorManager
{
	public static readonly Dictionary<LayerColor, Color> colors = new Dictionary<LayerColor, Color>()
	{
		{ LayerColor.Blue,   new Color(0   / 255f, 127 / 255f, 253 / 255f) },
		{ LayerColor.Green,  new Color(0   / 255f, 255 / 255f, 8   / 255f) },
		{ LayerColor.Red,    new Color(253 / 255f, 3   / 255f, 0   / 255f) },
		{ LayerColor.Yellow, new Color(255 / 255f, 245 / 255f, 0   / 255f) },
		{ LayerColor.Solid,  new Color(255 / 255f, 255 / 255f, 255 / 255f) },
	};
}
