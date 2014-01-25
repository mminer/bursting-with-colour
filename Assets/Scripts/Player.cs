using UnityEngine;

public class Player : MonoBehaviour
{
	public Layer layer { get; set; }

	void Start ()
	{
		LayerManager.AssignLayer(this);
		Debug.Log("Starting player on layer: " + layer.color);
	}
}
