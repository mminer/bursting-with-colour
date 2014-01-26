using UnityEngine;

/// <summary>
/// Heads-up display.
/// </summary>
public class HUD : MonoBehaviour
{
	public GUISkin guiSkin;
	Rect gameOverRect;

	void Update ()
	{
		DetermineGameOverRect();
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
		GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
				GUILayout.FlexibleSpace();

				if (GUILayout.Button("Play Again")) {
					Application.LoadLevel(Application.loadedLevelName);
				}

				if (GUILayout.Button("Quit")) {
					Application.Quit();
				}

				GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	void DetermineGameOverRect ()
	{
		gameOverRect = new Rect();
		gameOverRect.width = Screen.width / 4;
		gameOverRect.height = gameOverRect.width / 2;
		gameOverRect.x = (Screen.width / 2) - (gameOverRect.width / 2);
		gameOverRect.y = (Screen.height / 2) - (gameOverRect.height / 2);
	}
}
