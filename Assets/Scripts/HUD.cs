using UnityEngine;

/// <summary>
/// Heads-up display.
/// </summary>
public class HUD : MonoBehaviour
{
	public GUISkin guiSkin;
	Rect screenRect;

	void Awake ()
	{
		screenRect = new Rect(0, 0, Screen.width, Screen.height);
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
		// Center vertically.
		GUILayout.BeginArea(screenRect);
		GUILayout.FlexibleSpace();

		// Center horizontally.
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

			GUILayout.BeginVertical(GUI.skin.box);

				GUILayout.Label("Game Over", GUI.skin.GetStyle("gameOverLabel"));

				if (GUILayout.Button("Play Again")) {
					PlayerManager.ReplayLevel();
				}

				if (GUILayout.Button("Quit")) {
					Application.Quit();
				}

			GUILayout.EndVertical();

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}
}
