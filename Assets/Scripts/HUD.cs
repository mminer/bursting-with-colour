using UnityEngine;

/// <summary>
/// Heads-up display.
/// </summary>
public class HUD : MonoBehaviour
{
	public GUISkin guiSkin;
	Rect gameOverRect;

	void Awake ()
	{
		gameOverRect = GetGameOverRect();
	}

	void OnGUI ()
	{
		GUI.skin = guiSkin;
		ShowLives();
		ShowScore();

		if (PlayerManager.gameOver) {
			ShowGameOver();
		}
	}

	void ShowLives ()
	{
		GUILayout.Label("Lives: " + PlayerManager.lives);
	}

	void ShowScore ()
	{
		GUILayout.Label("Score: " + ScoreManager.score);
	}

	void ShowGameOver ()
	{
		gameOverRect = GUILayout.Window(0, gameOverRect, GameOverWindow, "Game Over");
	}

	void GameOverWindow (int windowID)
	{
		if (GUILayout.Button("Play Again")) {
			PlayerManager.ReplayLevel();
		}

		if (GUILayout.Button("Quit")) {
			Application.Quit();
		}
	}

	Rect GetGameOverRect ()
	{
		var width = Screen.width / 2;
		var height = Screen.height / 2;
		var x = (Screen.width / 2) - (width );
		var y = (Screen.height / 2) - (height);
		var rect = new Rect(width, height, x, y);
		return rect;
	}
}
