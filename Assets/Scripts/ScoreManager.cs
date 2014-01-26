using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	public static int score;
	static float scorePerUnit = 1f;
	static Vector3[] playerPositions;
	static float startingHeight;

	void Awake ()
	{
		// Start tracking player positions
		var players = GameObject.FindGameObjectsWithTag("Player");
		playerPositions = new Vector3[players.Length];

		for (int i = 0; i < players.Length; i++) {
			playerPositions[i] = players[i].transform.position;
		}

		startingHeight = playerPositions[0].y;
	}

	void Update ()
	{
		// Check player positions for new best score
		foreach (var player in playerPositions) {
			CheckScore(player.y);
		}
	}

	public static void CheckScore (float height)
	{
		var currentScore = Mathf.RoundToInt((height - startingHeight) * scorePerUnit);

		if (currentScore > score) {
			score = currentScore;
		}
	}
}
