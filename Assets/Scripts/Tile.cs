using UnityEngine;

public class Tile : MonoBehaviour
{
	public Layer layer { private get; set; }

	public TileKillBox killBox;
//	private bool checkForKill = false;

	public void Enable ()
	{
//		Debug.Log ("Enable being called");
		collider2D.enabled = true;
		renderer.material.color = ColorManager.activeColors[layer.color];
		animation.Play();

		//check for a kill collision next fixedupdate
//		killBox.CheckForKill();
	}

	public void Disable ()
	{
		collider2D.enabled = false;
		renderer.material.color = ColorManager.inactiveColors[layer.color];
	}


}
