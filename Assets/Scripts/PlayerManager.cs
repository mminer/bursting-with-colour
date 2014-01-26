using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public int playerLives = 2;
	public static bool gameOver = false;

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
		gameOver = true;
	}

	public static void ReplayLevel ()
	{
		gameOver = false;
		Application.LoadLevel(Application.loadedLevelName);
	}
}
