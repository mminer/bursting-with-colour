using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ColorManager
{
	const float inactiveAlpha = 0.3f;

	public static readonly Dictionary<LayerColor, Color> activeColors = new Dictionary<LayerColor, Color>()
	{
		{ LayerColor.Blue,   new Color(0   / 255f, 127 / 255f, 253 / 255f) },
		{ LayerColor.Green,  new Color(0   / 255f, 255 / 255f, 8   / 255f) },
		{ LayerColor.Red,    new Color(253 / 255f, 3   / 255f, 0   / 255f) },
		{ LayerColor.Yellow, new Color(255 / 255f, 245 / 255f, 0   / 255f) },
		{ LayerColor.Solid,  new Color(150 / 255f, 150 / 255f, 150 / 255f) },
	};

	public static readonly Dictionary<LayerColor, Color> inactiveColors;

	static ColorManager ()
	{
		inactiveColors = activeColors
			.Select(kvp => new KeyValuePair<LayerColor, Color>(kvp.Key, GetFadedColor(kvp.Value)))
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
	}

	static Color GetFadedColor (Color color)
	{
		var fadedColor = new Color(color.r, color.g, color.b, inactiveAlpha);
		return fadedColor;
	}
}
