using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controls layer visiblity.
/// </summary>
public class LayerManager : MonoBehaviour
{
	#region Public Inspector Fields

	public Material activeBlueMaterial;
	public Material activeGreenMaterial;
	public Material activeRedMaterial;
	public Material activeYellowMaterial;

	public Material inactiveBlueMaterial;
	public Material inactiveGreenMaterial;
	public Material inactiveRedMaterial;
	public Material inactiveYellowMaterial;

	#endregion

	public static Layer blueLayer { get; private set;}
	public static Layer greenLayer { get; private set; }
	public static Layer redLayer { get; private set; }
	public static Layer yellowLayer { get; private set; }

	public static Dictionary<LayerColor, Material> activeMaterials { get; private set; }
	public static Dictionary<LayerColor, Material> inactiveMaterials { get; private set; }

	static Layer[] layers;

	void Awake ()
	{
		SetMaterials();

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

	void SetMaterials ()
	{
		activeMaterials = new Dictionary<LayerColor, Material>()
		{
			{ LayerColor.Blue, activeBlueMaterial },
			{ LayerColor.Green, activeGreenMaterial },
			{ LayerColor.Red, activeRedMaterial },
			{ LayerColor.Yellow, activeYellowMaterial },
		};

		inactiveMaterials = new Dictionary<LayerColor, Material>()
		{
			{ LayerColor.Blue, inactiveBlueMaterial },
			{ LayerColor.Green, inactiveGreenMaterial },
			{ LayerColor.Red, inactiveRedMaterial },
			{ LayerColor.Yellow, inactiveYellowMaterial },
		};
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
