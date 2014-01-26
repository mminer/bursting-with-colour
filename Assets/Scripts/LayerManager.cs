using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controls layer visiblity.
/// </summary>
public class LayerManager : MonoBehaviour
{
	#region Public Inspector Fields

	public Material baseMaterial;

	#endregion

	public static Dictionary<LayerColor, Layer> layers { get; private set; }
	public static Dictionary<LayerColor, Material> activeMaterials { get; private set; }
	public static Dictionary<LayerColor, Material> inactiveMaterials { get; private set; }

	void Awake ()
	{
		layers = new Dictionary<LayerColor, Layer>();
		activeMaterials = new Dictionary<LayerColor, Material>();
		inactiveMaterials = new Dictionary<LayerColor, Material>();

		var layerComponents = GameObject.FindGameObjectsWithTag("Layer")
			.Select(go => go.GetComponent<Layer>())
			.ToArray();

		foreach (var color in System.Enum.GetValues(typeof(LayerColor)).Cast<LayerColor>()) {
			layers[color] = layerComponents.First(layer => layer.color == color);
			activeMaterials[color] = GetActiveMaterial(color);
			inactiveMaterials[color] = GetInactiveMaterial(color);
		}
	}

	Material GetActiveMaterial (LayerColor color)
	{
		var material = new Material(baseMaterial);
		material.color = ColorManager.colors[color];
		return material;
	}

	Material GetInactiveMaterial (LayerColor color)
	{
		var material = new Material(baseMaterial);
		var materialColor = ColorManager.colors[color];
		material.color = new Color(materialColor.r, materialColor.g, materialColor.b, 0.2f);
		return material;
	}

	public static void AssignLayer (Player player)
	{
		var inactiveLayers = layers
			.Where(kvp => !kvp.Value.active)
			.Select(kvp => kvp.Value)
			.ToArray();

		var layer = inactiveLayers[Random.Range(0, inactiveLayers.Length -1)];
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
