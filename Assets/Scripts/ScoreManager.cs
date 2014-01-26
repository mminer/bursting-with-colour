using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	public static int score;
	static float scorePerUnit = 1f;
	static Transform[] playerPositions;
	static float startingHeight;

	void Awake ()
	{
		// Start tracking player positions
		var players = GameObject.FindGameObjectsWithTag("Player");
		playerPositions = new Transform[players.Length];

		for (int i = 0; i < players.Length; i++) {
			playerPositions[i] = players[i].transform;
		}

		startingHeight = playerPositions[0].position.y;
	}

	void Update ()
	{
		if (PlayerManager.gameOver) {
			return;
		}

		// Check player positions for new best score
		foreach (var player in playerPositions) {
			if (player != null) {
				CheckScore(player.position.y);
			}
		}
	}

	public static void CheckScore (float height)
	{
		var calculatedScore = (height - startingHeight) * scorePerUnit;
		var currentScore = Mathf.RoundToInt(calculatedScore);

		Debug.Log("Raw: " + height + " Precise: " + calculatedScore + " Rounded: " + currentScore + " ::: " +Time.time);

		if (currentScore > score) {
			score = currentScore;
		}
	}
}
