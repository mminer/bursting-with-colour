using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour {

	public ParticleSystem particles;

	public void PlayerKilled()
	{
		if (particles != null)
		{
			Instantiate(particles, transform.position, Quaternion.identity);
			particles.startColor = GetPlayerColor();
			Destroy (gameObject);
		}
	}

	Color GetPlayerColor()
	{
		Color color = ColorManager.colors[LayerColor.Solid];

		Player playerScript = transform.GetComponent<Player>();
		if (playerScript != null)
			color = ColorManager.colors[playerScript.layer.color];

		return color;
	}
}
