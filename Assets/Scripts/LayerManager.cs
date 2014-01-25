using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controls layer visiblity.
/// </summary>
public class LayerManager : MonoBehaviour
{
	public static Layer blueLayer { get; private set;}
	public static Layer greenLayer { get; private set; }
	public static Layer redLayer { get; private set; }
	public static Layer yellowLayer { get; private set; }

	static Layer[] layers;

	void Awake ()
	{
		layers = GameObject.FindGameObjectsWithTag("Layer")
			.Select(go => go.GetComponent<Layer>())
			.ToArray();

		// Find named layers.
		blueLayer = layers.First(layer => layer.color == LayerColor.Blue);
		greenLayer = layers.First(layer => layer.color == LayerColor.Green);
		redLayer = layers.First(layer => layer.color == LayerColor.Red);
		yellowLayer = layers.First(layer => layer.color == LayerColor.Yellow);

		// Ensure all layers start out disabled.
		foreach (var layer in layers) {
			layer.active = false;
		}
	}

	public static void AssignLayer (Player player)
	{
		var inactiveLayers = GetInactiveLayers();
		var layer = inactiveLayers[Random.Range(0, inactiveLayers.Length)];
		layer.active = true;
		player.layer = layer;
	}

	public static void ToggleLayer (Player player, Layer newLayer)
	{
		if (newLayer.active) {
			Debug.LogWarning("Trying to switch to layer that's already active.");
			return;
		}

		var oldLayer = player.layer;

		Debug.Log(string.Format("Toggling layer from {0} to {1}.", oldLayer.color, newLayer.color));

		oldLayer.active = false;
		newLayer.active = true;
		player.layer = newLayer;
	}

	static Layer[] GetInactiveLayers ()
	{
		var inactiveLayers = layers.Where(layer => !layer.active).ToArray();
		return inactiveLayers;
	}
}
