using UnityEngine;

public enum LayerColor { Blue, Green, Red, Yellow, Solid }

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
			_active = value;
//			if (color == LayerColor.Solid) _active = true;
			ToggleChildren();
		}
	}

	public void ToggleChildren ()
	{
		foreach (Transform child in transform) {
			if (color == LayerColor.Solid)
				child.collider2D.enabled = true;
			else
				child.collider2D.enabled = active;
			child.renderer.material = active ? activeMaterial : inactiveMaterial;
		}
	}
}
