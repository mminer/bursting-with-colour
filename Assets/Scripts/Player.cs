using UnityEngine;

public class Player : MonoBehaviour
{
	public Layer layer { get; set; }

	void Start ()
	{
		LayerManager.AssignLayer(this);
		Debug.Log("Starting player on layer: " + layer.color);
	}

	void Update ()
	{
		// Temporary:

		/*
		if (Input.GetKey(KeyCode.Alpha1) && !LayerManager.blueLayer.active) {
			LayerManager.ToggleLayer(this, LayerManager.blueLayer);
		}

		if (Input.GetKey(KeyCode.Alpha2) && !LayerManager.greenLayer.active) {
			LayerManager.ToggleLayer(this, LayerManager.greenLayer);
		}

		if (Input.GetKey(KeyCode.Alpha3) && !LayerManager.redLayer.active) {
			LayerManager.ToggleLayer(this, LayerManager.redLayer);
		}

		if (Input.GetKey(KeyCode.Alpha4) && !LayerManager.yellowLayer.active) {
			LayerManager.ToggleLayer(this, LayerManager.yellowLayer);
		}
		*/
	}
}
