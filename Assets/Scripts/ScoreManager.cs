using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	public static int score;
	static float scorePerUnit = 1f;
	static Transform[] playerPositions;
	static float startingHeight;
	public static int highScore;
	static Transform scoreMarker;
	static Transform highScoreMarker;

	const string highScoreKey = "high_score";


	void Awake () 
	{
		Init();
	}

	void OnLevelWasLoaded (int level)
	{
		Init();
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

	static void Init ()
	{
		score = 0;
		GetStartingHeight();
		CheckHighScore();
		GetScoreMarker();
	}

	static void CheckHighScore ()
	{
		if (!PlayerPrefs.HasKey(highScoreKey)) {
			PlayerPrefs.SetInt(highScoreKey, 0);
		}
		
		highScore = PlayerPrefs.GetInt(highScoreKey);
	}

	static void GetStartingHeight ()
	{
		// Start tracking player positions
		var players = GameObject.FindGameObjectsWithTag("Player");
		playerPositions = new Transform[players.Length];
		
		for (int i = 0; i < players.Length; i++) {
			playerPositions[i] = players[i].transform;
		}
		
		startingHeight = playerPositions[0].position.y;
	}

	static void GetScoreMarker ()
	{
		scoreMarker = GameObject.Find("Score Marker").transform;
		highScoreMarker = GameObject.Find("High Score Marker").transform;
		highScoreMarker.position = new Vector3(highScoreMarker.position.x, highScore, 0);
	}

	public static void CheckScore (float height)
	{
		var calculatedScore = (height - startingHeight) * scorePerUnit;
		var currentScore = Mathf.RoundToInt(calculatedScore);

		if (currentScore > score) {
			score = currentScore;
			scoreMarker.position = new Vector3(scoreMarker.position.x, height, 0);

			if (score > highScore) {
				highScore = score;
				PlayerPrefs.SetInt(highScoreKey, highScore);
				highScoreMarker.position = new Vector3(highScoreMarker.position.x, highScore, 0);
			}
		}
	}
}
