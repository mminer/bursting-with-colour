using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public int playerLives = 10;

	public static int lives
	{
		get { return instance.playerLives; }
		set { instance.playerLives = value; }
	}

	static PlayerManager instance;

	void Awake ()
	{
		instance = this;
	}
}
