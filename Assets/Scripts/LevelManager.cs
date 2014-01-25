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
		for (int y = 0; y < 100; y += 5) {
			for (int x = 0; x < gridWidth; x++) {
				// Only create a new tile a specified percent of the time.
				if (Random.Range(0, 100) > 40) {
					continue;
				}

				// Randomly decide which color to make tile.
				var color = (LayerColor)Random.Range(0, LayerManager.layers.Count);
				var position = new Vector2(x * 10, y * 10);
				tile = Instantiate(tile, position, Quaternion.identity) as Transform;
				tile.parent = LayerManager.layers[color].transform;
			}
		}
	}
}
