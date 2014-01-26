using UnityEngine;
using System.Collections;

public class PlayerOverlapCheck : MonoBehaviour {

	void Update()
	{
		LayerMask mask = ~(1 << 9);
		Vector2 start = new Vector2(transform.position.x, transform.position.y);
		RaycastHit2D hit = Physics2D.Raycast(start, -Vector2.up, transform.parent.localScale.y * 0.8f, mask);

		if (hit.collider != null) {
			SendMessageUpwards("PlayerKilled");
		}
	}
}
