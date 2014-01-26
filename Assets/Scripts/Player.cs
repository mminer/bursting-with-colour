using UnityEngine;

public class Player : MonoBehaviour
{
	public int playerID = 1;
	public GameObject p1Label;
	public GameObject p2Label;

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

		p1Label.SetActive(playerID == 1);
		p2Label.SetActive(playerID == 2);
	}
}
