using UnityEngine;

/// <summary>
/// Heads-up display.
/// </summary>
public class HUD : MonoBehaviour
{
	void OnGUI ()
	{
		GUILayout.Label("Lives: " + Globals.instance.playerLives);
	}
}
