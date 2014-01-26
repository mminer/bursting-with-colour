using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controls layer visiblity.
/// </summary>
public class LayerManager : MonoBehaviour
{
	public static Dictionary<LayerColor, Layer> layers { get; private set; }

	void Awake ()
	{
		layers = GameObject.FindGameObjectsWithTag("Layer")
			.Select(go => go.GetComponent<Layer>())
			.ToDictionary(layer => layer.color);

		// Ensure solid "colour" is always active.
		layers[LayerColor.Solid].active = true;
	}

	public static void AssignLayer (Player player)
	{
		var inactiveLayers = layers
			.Where(kvp => !kvp.Value.active)
			.Select(kvp => kvp.Value)
			.ToArray();

		var layer = inactiveLayers[Random.Range(0, inactiveLayers.Length)];
		layer.active = true;
		player.layer = layer;
	}

	public static void ToggleLayer (GameObject playerGameObject, LayerColor newLayerColor)
	{
		var player = playerGameObject.GetComponent<Player>();
		var oldLayer = player.layer;
		var newLayer = layers[newLayerColor];

		if (newLayer.active) {
			Debug.LogWarning("Trying to switch to layer that's already active.");
			return;
		}

		Debug.Log(string.Format("Toggling layer from {0} to {1}.", oldLayer.color, newLayer.color));

		oldLayer.active = false;
		newLayer.active = true;
		player.layer = newLayer;
	}
}
