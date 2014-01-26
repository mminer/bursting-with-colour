using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public int playerLives = 2;
	public static bool gameOver = false;
	public static GameObject[] players;

	public static int lives
	{
		get { return instance.playerLives; }
		set { 
			instance.playerLives = value; 
		}
	}

	static PlayerManager instance;

	void Awake ()
	{
		instance = this;
		players = GameObject.FindGameObjectsWithTag("Player");
		gameOver = false;
	}

	public static void LoseLife ()
	{
		lives--;
	
		if (lives <= 0) {
			DoGameOver();
		}
	}

	public static void DoGameOver ()
	{
		ScoreManager.SetFinalScore();
		gameOver = true;
	}

	public static void ReplayLevel ()
	{
		gameOver = false;
		Application.LoadLevel(Application.loadedLevelName);
	}
}
