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
		Color color = ColorManager.activeColors[LayerColor.Solid];

		Player playerScript = transform.GetComponent<Player>();
		if (playerScript != null)
			color = ColorManager.activeColors[playerScript.layer.color];

		return color;
	}
}
