using UnityEngine;
using System.Collections;

public class KillTrigger : MonoBehaviour
{
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.CompareTag("Player")) {
			col.gameObject.SendMessage("PlayerKilled");
		}
	}
}
