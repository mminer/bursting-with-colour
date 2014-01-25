using UnityEngine;

public enum LayerColor { Blue, Green, Red, Yellow }

public class Layer : MonoBehaviour
{
	public LayerColor color;
	
	Material activeMaterial { get { return LayerManager.activeMaterials[color]; } }
	Material inactiveMaterial { get { return LayerManager.inactiveMaterials[color]; } }
	
	bool _active;
	public new bool active
	{
		get { return _active; }
		set {
			ToggleChildren(value);
			_active = value;
		}
	}
	
	void ToggleChildren (bool active)
	{
		foreach (Transform child in transform) {
			child.collider2D.enabled = active;
			child.renderer.material = active ? activeMaterial : inactiveMaterial;
		}
	}
}