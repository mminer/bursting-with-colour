using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public int playerLives = 10;
	public static bool gameOver;
	static Platformer2DUserControl[] userControls;

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
		var players = GameObject.FindGameObjectsWithTag("Player");
		userControls = new Platformer2DUserControl[players.Length];

		for (int i = 0; i < players.Length; i++) {
			userControls[i] = players[i].GetComponent<Platformer2DUserControl>();
		}
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
		
		foreach (var controller in userControls) {
			controller.enabled = false;
		}
	}
}
