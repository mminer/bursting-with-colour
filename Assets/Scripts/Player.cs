using UnityEngine;

public class Player : MonoBehaviour
{
	public int playerID = 1;
	private SpriteRenderer spriteRenderer;
	private Layer _layer;
	public Layer layer {
		get { return _layer; }
		set {
			_layer = value;
			SetColor(value.color);
		}
	}

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start ()
	{
		LayerManager.AssignLayer(this);
		Debug.Log("Starting player on layer: " + layer.color);
	}

	void SetColor (LayerColor c)
	{
		Color newColor = Color.magenta;

		switch (c) {
			case LayerColor.Blue: 		newColor = Color.blue; 		break;
			case LayerColor.Green: 		newColor = Color.green; 	break;
			case LayerColor.Red: 		newColor = Color.red; 		break;
			case LayerColor.Yellow:		newColor = Color.yellow; 	break;
		}

		spriteRenderer.color = newColor;
	}
}
