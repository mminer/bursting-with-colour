using UnityEngine;

/// <summary>
/// Controls level generation.
/// </summary>
public class LevelManager : MonoBehaviour
{
	public Tile tile;
	public int gridWidth = 20;
	public int mapHeight = 100;
	public int safeZoneWidth = 5;
	public float chanceOfBlankTile = 0.2f;
	public float chanceOfNeighbourColour = 0.5f;

	public enum Layout { Solid, Rows };
	public Layout levelLayout;
	int rowsBeforeSafety = 3;

	void Start ()
	{
		GenerateBorders();
		GenerateGround();
		GenerateTiles();
		InitializeLayerChildren();
		if ((chanceOfBlankTile < 0f) || (chanceOfBlankTile > 1f)) chanceOfBlankTile = 0.2f;
	}

	void GenerateBorders()
	{
		//bottom ground
		for (int x = -safeZoneWidth; x < gridWidth; x++)
		{
			CreateTile(LayerColor.Solid, x, -5);
		}

		//bottom left wall
		for (int y = -4; y < 0; y++)
		{
			CreateTile(LayerColor.Solid, -safeZoneWidth, y);
		}

		//bottomrightwall
		for (int y = -5; y < 0; y++)
		{
			CreateTile(LayerColor.Solid, gridWidth, y);
		}

		//bottom overhang
		for (int x = -4; x < 0; x++)
		{
			CreateTile(LayerColor.Solid, x, -1);
		}

		//tallwalls
		for (int y = 0; y < mapHeight; y++)
		{
			CreateTile(LayerColor.Solid, -1, y);
		}
		for (int y = 0; y < mapHeight; y++)
		{
			CreateTile(LayerColor.Solid, gridWidth, y);
		}
	}

	void GenerateGround ()
	{
		//steps
		CreateTile(LayerColor.Blue, 5, -4);
		CreateTile(LayerColor.Blue, 6, -4);
		CreateTile(LayerColor.Green, 6, -3);
		CreateTile(LayerColor.Blue, 7, -4);
		CreateTile(LayerColor.Green, 7, -3);
		CreateTile(LayerColor.Red, 7, -2);
		//rest of them
		for (int x = 8; x < gridWidth; x++) {
			CreateTile(LayerColor.Blue,   x, -4);
			CreateTile(LayerColor.Green,  x, -3);
			CreateTile(LayerColor.Red,    x, -2);
			CreateTile(LayerColor.Yellow, x, -1);
		}
	}

	void GenerateTiles ()
	{
		for (int y = 0; y < mapHeight; y++) {
			LayerColor lastColor = LayerColor.Solid;

			if (levelLayout == Layout.Rows) {
				if (y % rowsBeforeSafety == 0) {
					continue;
				}
			}

			for (int x = 0; x < gridWidth; x++) {
				// Only create a new tile a specified percent of the time.

				if (Random.Range(0f, 1f) < chanceOfBlankTile) {
					continue;
				}

				LayerColor tileColor = LayerColor.Solid;

				// Randomly decide if this tile should share its neighbours color
				if (lastColor != LayerColor.Solid) {
					if (Random.Range(0f, 1f) < chanceOfNeighbourColour) {
						tileColor = lastColor;
					}
				}

				// Randomly decide which color to make tile.
				if (tileColor == LayerColor.Solid) {
					tileColor = (LayerColor)Random.Range(0, LayerManager.layers.Count - 1);
					lastColor = tileColor;
				}

				CreateTile(tileColor, x, y);
			}
		}
	}

	void CreateTile (LayerColor color, int x, int y)
	{
		var position = new Vector2(x, y);
		var newTile = Instantiate(tile, position, Quaternion.identity) as Tile;
		var layer = LayerManager.layers[color];
		newTile.layer = layer;
		newTile.transform.parent = layer.transform;
	}

	static void InitializeLayerChildren ()
	{
		foreach (var layer in LayerManager.layers.Values) {
			layer.ToggleChildren();
		}
	}
}
