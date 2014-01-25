using UnityEngine;
using System.Collections.Generic;

public static class ColorManager
{
	public static readonly Dictionary<LayerColor, Color> colors = new Dictionary<LayerColor, Color>()
	{
		{ LayerColor.Blue,   new Color(103 / 255f, 35f  / 255f, 127f / 255f) },
		{ LayerColor.Green,  new Color(50  / 255f, 157f / 255f, 39f  / 255f) },
		{ LayerColor.Red,    new Color(188 / 255f, 47f  / 255f, 54f  / 255f) },
		{ LayerColor.Yellow, new Color(180 / 255f, 189f / 255f, 47f  / 255f) },
	};
}
