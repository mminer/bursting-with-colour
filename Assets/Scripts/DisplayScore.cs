using UnityEngine;
using System.Collections;

public class DisplayScore : MonoBehaviour {

	void OnGUI ()
	{
		GUI.skin = HUD.skin;
		var screenPos = Camera.main.WorldToScreenPoint(transform.position);
		var y = Screen.height - screenPos.y;
		var labelRect = new Rect(screenPos.x, y, 200, 100);

		int score = ScoreManager.score;
		string label = "Score: " + score;
		string bonus = "2P Bonus: " + ScoreManager.bonus;
		GUI.Label(labelRect, label + "\n" + bonus);
	}
}
