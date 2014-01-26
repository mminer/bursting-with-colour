using UnityEngine;
using System.Collections;

public class PlayerOverlapCheck : MonoBehaviour {

//	void OnCollisionEnter2D(Collision2D col)
//	{
//		if (!col.gameObject.CompareTag("Player"))
//		{
//			Debug.Log (col.gameObject.name);
//			col.gameObject.SendMessage("PlayerKilled");
//		}
//	}
	LayerMask mask = ~(1 << 9);
	public void CheckOverlap()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, transform.parent.localScale.y * 0.8f, mask))
			Debug.Log ("Raycast hit! " + hit.collider.gameObject.name);
	}
}
