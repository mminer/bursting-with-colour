using UnityEngine;
using System.Collections;

public class TileKillBox : MonoBehaviour {

	private bool checkForKill = false;

	private bool stopCheckForKill = false;

	public void CheckForKill()
	{
		checkForKill = true;
		stopCheckForKill = true;
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (checkForKill)
		{
			Debug.Log ("Checking for a kill...");
			checkForKill = false;
			if (col.gameObject.CompareTag("Player"))
			{
				Debug.Log ("Killing the player!");
				col.gameObject.SendMessage("PlayerKilled");
			}
		}

		stopCheckForKill = true;
	}

	void FixedUpdate()
	{
		if (stopCheckForKill) 
		{
			checkForKill = false;
			stopCheckForKill = false;
		}
	}
}
