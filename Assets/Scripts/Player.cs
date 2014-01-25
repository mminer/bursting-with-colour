using UnityEngine;

public class Player : MonoBehaviour
{
	public int playerID = 1;

	SpriteRenderer spriteRenderer;

	Layer _layer;
	public Layer layer {
		get { return _layer; }
		set {
			_layer = value;
			SetColor();
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
	}

	void SetColor ()
	{
		spriteRenderer.color = ColorManager.colors[layer.color];
		Debug.Log("Sprite color: " + spriteRenderer.color);
	}
}
