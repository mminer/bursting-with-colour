using UnityEngine;
using System.Collections;

public class KillTrigger : MonoBehaviour
{
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.name.IndexOf("Player") >= 0)
		{
			col.gameObject.SendMessage("PlayerKilled");
		}
	}
}
