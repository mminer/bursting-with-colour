using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	public static int score;
	public static int bonus;
	static float scorePerUnit = 1f;
	static Transform[] playerPositions;
	static float startingHeight;
	public static int highScore;
	static Transform scoreMarker;

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
		bonus = 0;
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
	}

	public static void CheckScore (float height)
	{
		var calculatedScore = (height - startingHeight) * scorePerUnit;
		var currentScore = Mathf.RoundToInt(calculatedScore);

		if (currentScore > score) {
			if (PlayerManager.lives > 1) {
				bonus += currentScore - score;
			}

			score = currentScore;
			scoreMarker.position = new Vector3(scoreMarker.position.x, height, 0);

			if (score > highScore) {
				highScore = score;
				PlayerPrefs.SetInt(highScoreKey, highScore);
			}
		}
	}

	public static void SetFinalScore ()
	{
		score = score + bonus;

		if (score > highScore) {
			highScore = score;
			PlayerPrefs.SetInt(highScoreKey, highScore);
		}
	}
}
