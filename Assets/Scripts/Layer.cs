using UnityEngine;

public enum LayerColor { Blue, Green, Red, Yellow, Solid }

public class Layer : MonoBehaviour
{
	public LayerColor color;

	bool _active;
	public new bool active
	{
		get { return _active; }
		set {
			_active = value;
			ToggleChildren();
		}
	}

	public void ToggleChildren ()
	{
		foreach (Transform child in transform) {
			if (active) {
				child.GetComponent<Tile>().Enable();
			} else {
				child.GetComponent<Tile>().Disable();
			}
		}
	}
}
