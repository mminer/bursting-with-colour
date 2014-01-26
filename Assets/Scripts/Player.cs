using UnityEngine;

public class Player : MonoBehaviour
{
	public int playerID = 1;
	public GameObject p1Label;
	public GameObject p2Label;

	GameObject activeLabel;
	SpriteRenderer spriteRenderer;

	Layer _layer;
	public Layer layer {
		get { return _layer; }
		set {
			_layer = value;
			spriteRenderer.color = ColorManager.activeColors[value.color];
		}
	}

	void Awake ()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start ()
	{
		LayerManager.AssignLayer(this);
		Debug.Log("Starting player on layer: " + layer.color);

		if (playerID == 1) {
			activeLabel = p1Label;
		} else {
			activeLabel = p2Label;
		}

		activeLabel.SetActive(true);
	}

	void Update ()
	{
		FlipLabel();
	}

	void FlipLabel ()
	{
		var currentScale = activeLabel.transform.localScale;

		if (transform.localScale.x < 0 && currentScale.x > 0) {
			var newScale = currentScale;
			newScale.x *= -1;
			activeLabel.transform.localScale = newScale;
		}
	}
}
