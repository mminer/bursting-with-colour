using UnityEngine;

public class Globals : MonoBehaviour
{
	public int playerLives = 10;

	public static Globals instance;

	void Awake ()
	{
		instance = this;
		DontDestroyOnLoad(instance);
	}
}
