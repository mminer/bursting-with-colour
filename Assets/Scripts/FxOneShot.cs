using UnityEngine;
using System.Collections;

public class FxOneShot : MonoBehaviour {

	void Start()
	{
		particleSystem.renderer.sortingLayerName = "Foreground";
	}

	// Update is called once per frame
	void Update () {
		if (!particleSystem.isPlaying)
			Destroy (gameObject);
	}
}
