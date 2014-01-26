using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour {

	public GameObject particles;
	Color color;

	public void PlayerKilled()
	{
		PlayerManager.LoseLife();
		AudioManager.PlayDeath();

		if (particles != null)
		{
			//instantiate the particles
			GameObject particlesGo = Instantiate(particles, transform.position + (Vector3.down * 1.3f), Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as GameObject;

			//set the particles color to match the player
			UpdatePlayerColor();

			ParticleSystem particlesPs = particlesGo.GetComponent<ParticleSystem>();
			if (particlesPs != null)
				particlesPs.startColor = color;

			//set the light flare color to match
			ParticleSystem childParticles = particlesGo.transform.GetChild(0).GetComponent<ParticleSystem>();
			if (childParticles != null)
			{
				//Debug.Log ("Setting " + childParticles.gameObject.name + " to " + color);
				childParticles.startColor = color;
			}

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
