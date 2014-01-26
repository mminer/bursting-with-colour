using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	public static int score;
	static float scorePerUnit = 1f;
	static Vector3 camPos;

	void Awake ()
	{
		camPos = Camera.main.transform.position;
	}

	public static void UpdateScore ()
	{
		var currentScore = Mathf.RoundToInt(camPos.y * scorePerUnit);

		if (currentScore > score) {
			score = currentScore;
		}
	}
}
