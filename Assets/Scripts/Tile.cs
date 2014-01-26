using UnityEngine;

public class Tile : MonoBehaviour
{
	public Layer layer { private get; set; }

	public void Enable ()
	{
		collider2D.enabled = true;
		renderer.material = layer.activeMaterial;
	}

	public void Disable ()
	{
		collider2D.enabled = false;
		renderer.material = layer.inactiveMaterial;
	}
}
