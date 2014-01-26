using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour {

	public GameObject particles;
	Color color;

	public void PlayerKilled()
	{
		PlayerManager.LoseLife();

		if (particles != null)
		{
			//instantiate the particles
			GameObject particlesGo = Instantiate(particles, transform.position, Quaternion.identity) as GameObject;

			//set the particles color to match the player
			UpdatePlayerColor();

			ParticleSystem particlesPs = particlesGo.GetComponent<ParticleSystem>();
			if (particlesPs != null)
				particlesPs.startColor = color;
			else
				Debug.Log ("Couldn't find the particle system!");
			//set the light flare color to match
			Light particlesLight = particlesGo.GetComponentInChildren<Light>();
			if (particlesLight != null)
				particlesLight.color = color;
			//destroy the player
			Destroy (gameObject);
		}
	}

	void UpdatePlayerColor()
	{
		color = ColorManager.activeColors[LayerColor.Solid];

		Player playerScript = transform.GetComponent<Player>();
		if (playerScript != null)
			color = ColorManager.activeColors[playerScript.layer.color];
	}
}
