using UnityEngine;

/// <summary>
/// Controls level generation.
/// </summary>
public class LevelManager : MonoBehaviour
{
	public Transform tile;
	public int gridWidth = 20;

	void Start ()
	{
		GenerateGround();
		GenerateTiles();
		InitializeLayerChildren();
	}

	void GenerateGround ()
	{
		for (int x = 0; x < gridWidth; x++) {
			CreateTile(LayerColor.Blue,   x, -4);
			CreateTile(LayerColor.Green,  x, -3);
			CreateTile(LayerColor.Red,    x, -2);
			CreateTile(LayerColor.Yellow, x, -1);
		}
	}

	void GenerateTiles ()
	{
		for (int y = 0; y < 100; y += 5) {
			for (int x = 0; x < gridWidth; x++) {
				// Only create a new tile a specified percent of the time.
				/*
				if (Random.Range(0, 100) > 40) {
					continue;
				}
				*/

				// Randomly decide which color to make tile.
				var color = (LayerColor)Random.Range(0, LayerManager.layers.Count);
				CreateTile(color, x, y);
			}
		}
	}

	void CreateTile (LayerColor color, int x, int y)
	{
		var position = new Vector2(x, y);
		tile = Instantiate(tile, position, Quaternion.identity) as Transform;
		tile.parent = LayerManager.layers[color].transform;
	}

	static void InitializeLayerChildren ()
	{
		foreach (var layer in LayerManager.layers.Values) {
			layer.ToggleChildren();
		}
	}
}
