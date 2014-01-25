using UnityEngine;

public enum LayerColor { Blue, Green, Red, Yellow }

public class Layer : MonoBehaviour
{
	public LayerColor color;

	bool _active;

	public new bool active
	{
		get { return _active; }
		set {
			EnableChildColliders(value);
			_active = value;
		}
	}

	void EnableChildColliders (bool active)
	{
		foreach (var childCollider in GetComponentsInChildren<Collider2D>()) {
			childCollider.enabled = active;
		}
	}
}
