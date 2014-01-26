using UnityEngine;

/// <summary>
/// Heads-up display.
/// </summary>
public class HUD : MonoBehaviour
{
	public GUISkin guiSkin;
	public static GUISkin skin;

	Rect screenRect;

	void Awake ()
	{
		screenRect = new Rect(0, 0, Screen.width, Screen.height);
		skin = guiSkin;
	}

	void Update ()
	{
		CheckPressedStart();
	}

	void OnGUI ()
	{
		GUI.skin = guiSkin;
		ShowScore();

		if (PlayerManager.gameOver) {
			ShowGameOver();
		}
	}

	void ShowScore ()
	{
		GUILayout.Label("Best: " + ScoreManager.highScore);
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

	void CheckPressedStart ()
	{
		var suffix = "Win";

		#if UNITY_STANDALONE_OSX
		suffix = "Mac";
		#endif

		var p1Start = GenerateInputName("Start", 1, suffix);
		var p2Start = GenerateInputName("Start", 2, suffix);

		if (Input.GetButtonDown(p1Start) || Input.GetButtonDown(p2Start)) {
			Application.LoadLevel(Application.loadedLevelName);
		}
	}

	string GenerateInputName (string prefix, int playerID, string suffix)
	{
		return prefix + "_" + playerID + "_" + suffix;
	}
}
